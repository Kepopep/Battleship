using Server.GameLogic.Field;
using Server.GameLogic.Player;

namespace Server.Web.Lobby.Game;

public struct GameFieldInfo
{
    public Guid Id { get; set; }

    private readonly OpponentInfo _firstInfo;

    private readonly OpponentInfo _secondInfo;

    public GameFieldInfo(OpponentInfo firstInfo, OpponentInfo secondInfo)
    {
        _firstInfo = firstInfo;
        _secondInfo = secondInfo;

    }

    public bool HasId(string connectionId)
    {
        return _firstInfo.ConnectionId == connectionId 
               || _secondInfo.ConnectionId == connectionId;
    }

    public Player? GetPlayer(string connectionId)
    {
        return _firstInfo.ConnectionId == connectionId ? _firstInfo.Player :
            _secondInfo.ConnectionId == connectionId ? _secondInfo.Player : null;
    }
    
    public OpponentInfo GetOpponentInfo(string connectionId)
    {
        return _firstInfo.ConnectionId == connectionId ? _secondInfo :
            _secondInfo.ConnectionId == connectionId ? _firstInfo : default;
    }
}