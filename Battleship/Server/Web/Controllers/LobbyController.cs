using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Server.Web.Hub;
using Server.Web.Lobby;

namespace Server.Web.Controllers;

[ApiController]
[Route("api/lobby")]
public class LobbyController : Controller
{
    private readonly IWaiterList _waitersList;
    
    private readonly IHubContext<GameHub, IGameHub> _gameHubContext;

    public LobbyController(IWaiterList waitersList, IHubContext<GameHub, IGameHub> gameHubContext)
    {
        _waitersList = waitersList;
        _gameHubContext = gameHubContext;
    }

    public async Task<IActionResult> Connect(string firstId, string secondId)
    {
        if (!await _waitersList.Contains(firstId))
        {
            return NotFound("No first waiter found");
        }
        
        if (!await _waitersList.Contains(secondId))
        {
            return NotFound("No second waiter found");
        }
        
        await _waitersList.Remove(firstId);
        await _waitersList.Remove(secondId);
        
        await _gameHubContext.Clients
            .Client(firstId)
            .LoadGame(secondId);
        
        await _gameHubContext.Clients
            .Client(secondId)
            .LoadGame(firstId);
        
        return Ok();
    }
}