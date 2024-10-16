using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Server.GameLogic.Field;
using Server.GameLogic.Player;
using Server.Web.DTO;
using Server.Web.Hub;
using Server.Web.Lobby;
using Server.Web.Lobby.Game;
using Server.Web.Misc;

namespace Server.Web.Controllers;

[ApiController]
[Route("api/game-lobby")]
public class GameLobbyController : Controller
{
    private readonly IHubContext<GameHub, IGameHub> _hubContext;
    
    private readonly IGameList _gameList;
    
    private readonly IConfiguration _configuration;

    public GameLobbyController(IHubContext<GameHub, IGameHub> hubContext, IGameList gameList, IConfiguration configuration)
    {
        _hubContext = hubContext;
        _gameList = gameList;
        _configuration = configuration;
    }
    
    [HttpPost("connect")]
    public async Task<IActionResult> Connect([FromBody]GameConnectPostInfo info)
    {
        var lobbyGuid = Guid.NewGuid();
        
        Console.WriteLine($"Game-lobby.Connect {info.FirstId} {info.SecondId}");
        
        var firstInfo = new OpponentInfo
        {
            Id = Guid.NewGuid(),
            ConnectionId = info.FirstId,
            Player = new Player(_configuration)
        };
        await _hubContext.ToGameGroup(firstInfo.ConnectionId, lobbyGuid.ToString());
        
        var secondInfo = new OpponentInfo
        {
            Id = Guid.NewGuid(),
            ConnectionId = info.SecondId,
            Player = new Player(_configuration)
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
        
        var player = gameFieldInfo.GetOpponentInfo(info.ConnectionId).Player;

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
        
        await _hubContext
            .Clients
            .Client(info.ConnectionId)
            .UpdateSelfField(fieldView);

        return Ok();
    }


    [HttpPost("shoot")]
    public async Task<IActionResult> Shoot([FromBody] ShootPostInfo info)
    {
        var gameFieldInfo = await _gameList.GetByOpponentId(info.ConnectionId);

        if (gameFieldInfo.Equals(default(GameFieldInfo)))
        {
            return NotFound();
        }

        var currentInfo = gameFieldInfo
            .GetOpponentInfo(info.ConnectionId);
        
        if (currentInfo.Player == null)
        {
            return NotFound();
        }

        var activeOpponent = gameFieldInfo.GetActiveOpponentInfo();

        if (currentInfo.Id != activeOpponent.Id)
        {
            Console.WriteLine($"Hit: Not active turn");
            return BadRequest();
        }

        if (currentInfo.Player.Shoot(info.CellIndex) == CellShooter.ShootResult.Missed)
        {
            gameFieldInfo.ChangeActiveOpponent();
        }
        
        var playerFieldView = new GameFieldView
        {
            Self = currentInfo.Player.GetSelfFieldView(),
            Opponent = currentInfo.Player.GetOpponentFieldView()
        };
        
        await _hubContext
            .Clients
            .Client(info.ConnectionId)
            .UpdateSelfField(playerFieldView);

        var enemyInfo = gameFieldInfo.GetEnemyOpponentInfo(info.ConnectionId);
        
        var opponentFieldView = new GameFieldView()
        {
            Self = enemyInfo.Player?.GetSelfFieldView() ?? Array.Empty<Cell>(),
            Opponent = enemyInfo.Player?.GetOpponentFieldView() ?? Array.Empty<Cell>()
        };
        
        await _hubContext
            .Clients
            .Client(enemyInfo.ConnectionId)
            .UpdateSelfField(opponentFieldView);
        
        return Ok();
    }
}
