using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Server.Web.Controllers.DTO;
using Server.Web.Hub;
using Server.Web.Lobby;

namespace Server.Web.Controllers;

[ApiController]
[Route("api/wait-lobby")]
public class WaitLobbyController : ControllerBase
{
    private static readonly HttpClient Client = new HttpClient();
    
    private readonly IWaiterList _waiterList;
    
    private readonly IHubContext<GameHub> _hubContext;

    public WaitLobbyController(IWaiterList waiterList, IHubContext<GameHub> hubContext)
    {
        _waiterList = waiterList;
        _hubContext = hubContext;
    }
    
    [HttpPost("connect/{id}")]
    public async Task<IActionResult> Connect(string id)
    {
        await _hubContext.Groups.AddToGroupAsync(id, "wait-lobby");
        await _waiterList.Add(id);

        var pairInfo = await GetPair(id);

        if (pairInfo.PairStatus != WaiterListPair.Status.HasPair)
        {
            return Ok();
        }
        
        var pareInfo = new GameLobbyPair()
        {
            FirstId = id,
            SecondId = pairInfo.PairId
        };
            
        await Client.PostAsync($"http://localhost:5000/api/game-lobby/connect", 
            JsonContent.Create(pareInfo));

        return Ok();
    }
    
    [HttpPost("disconnect/{id}")]
    public async Task<IActionResult> Disconnect(string id)
    {
        await _hubContext.Groups.RemoveFromGroupAsync(id, "wait-lobby");
        await _waiterList.Remove(id);
        
        return Ok();
    }

    [HttpGet("pair/{id}")]
    public async Task<WaiterListPair> GetPair(string id)
    {
        var allWaiters = await _waiterList.GetAll();

        var pairCandidate = allWaiters
            .Where(x => x != id)
            .ToList();

        if (pairCandidate.Count == 0)
        {
            return new WaiterListPair()
            {
                PairStatus = WaiterListPair.Status.Alone
            };
        }
        
        var pairId = pairCandidate.First();
            
        await _waiterList.Remove(pairId);
            
        return new WaiterListPair()
        {
            PairId = pairId,
            PairStatus = WaiterListPair.Status.HasPair
        };
    }
}