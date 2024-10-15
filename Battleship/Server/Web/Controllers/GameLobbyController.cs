using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Server.GameLogic.Field;
using Server.GameLogic.Player;
using Server.GameLogic.Ship;
using Server.Web.Controllers.DTO;
using Server.Web.Controllers.Misc;
using Server.Web.Hub;
using Server.Web.Hub.DTO;
using Server.Web.Lobby.Game;

namespace Server.Web.Controllers;

[ApiController]
[Route("api/game-lobby")]
public class GameLobbyController : Controller
{
    private readonly IHubContext<GameHub, IGameHub> _hubContext;
    private readonly IGameList _gameList;

    public GameLobbyController(IHubContext<GameHub, IGameHub> hubContext, IGameList gameList)
    {
        _hubContext = hubContext;
        _gameList = gameList;
    }
    
    [HttpPost("connect")]
    public async Task<IActionResult> Connect([FromBody]GameLobbyPair pairInfo)
    {
        var lobbyGuid = Guid.NewGuid();
        
        Console.WriteLine($"Game-lobby.Connect {pairInfo.FirstId} {pairInfo.SecondId}");
        
        var firstInfo = new OpponentInfo
        {
            Id = Guid.NewGuid(),
            ConnectionId = pairInfo.FirstId,
            Player = new Player()
        };
        await _hubContext.ToGameGroup(firstInfo.ConnectionId, lobbyGuid.ToString());
        
        var secondInfo = new OpponentInfo
        {
            Id = Guid.NewGuid(),
            ConnectionId = pairInfo.SecondId,
            Player = new Player()
        };
        await _hubContext.ToGameGroup(secondInfo.ConnectionId, lobbyGuid.ToString());

        var gameField = new GameFieldInfo(firstInfo, secondInfo)
        {
            Id = lobbyGuid
        };
        
        await _gameList.Add(gameField);
        
        firstInfo.Player.AssignEnemy(secondInfo.Player);
        secondInfo.Player.AssignEnemy(firstInfo.Player);
        
        Console.WriteLine($"Game-lobby.Connect / CREATED {lobbyGuid}");
        
        await _hubContext
            .Clients
            .Group(lobbyGuid.ToString())
            .LoadGame();
        
        return Ok();
    }
    
    [HttpPost("place")]
    public async Task<IActionResult> Place([FromBody]PlacePostInfo info)
    {
        var gameFieldInfo = await _gameList.GetByOpponentId(info.ConnectionId);
        
        var player = gameFieldInfo.GetPlayer(info.ConnectionId);

        if (player == null)
        {
            return NotFound();
        }
        
        player.Place(info.CellIndex, info.Type, info.IsVertical);

        var fieldView = new GameFieldView
        {
            Self = player.GetSelfFieldView(),
            Opponent = player.GetOpponentFieldView()
        };
        
        await _hubContext.Clients
            .User(info.ConnectionId)
            .UpdateSelfField(fieldView);

        return Ok();
    }
}
