using Server.GameLogic.Field;
using Server.GameLogic.Player;
using Server.Web.Lobby;

namespace Server.Web.Misc;

public class GameFieldInfo
{
    public Guid Id { get; set; }

    private readonly OpponentInfo _firstInfo;

    private readonly OpponentInfo _secondInfo;
    
    private Guid _activeGuid;

    public GameFieldInfo(OpponentInfo firstInfo, OpponentInfo secondInfo)
    {
        _firstInfo = firstInfo;
        _secondInfo = secondInfo;
        
        _activeGuid = _firstInfo.Id;
    }

    public bool HasId(string connectionId)
    {
        return _firstInfo.ConnectionId == connectionId 
               || _secondInfo.ConnectionId == connectionId;
    }

    public OpponentInfo GetOpponentInfo(string connectionId)
    {
        return _firstInfo.ConnectionId == connectionId ? _firstInfo :
            _secondInfo.ConnectionId == connectionId ? _secondInfo : default;
    }
    
    public OpponentInfo GetEnemyOpponentInfo(string connectionId)
    {
        return _firstInfo.ConnectionId == connectionId ? _secondInfo :
            _secondInfo.ConnectionId == connectionId ? _firstInfo : default;
    }

    public OpponentInfo GetActiveOpponentInfo()
    {
        return _firstInfo.Id == _activeGuid ? _firstInfo : 
            _secondInfo.Id == _activeGuid ? _secondInfo : default;
    }

    public void ChangeActiveOpponent()
    {
        _activeGuid = _firstInfo.Id == _activeGuid ? _secondInfo.Id : 
            _secondInfo.Id == _activeGuid ? _firstInfo.Id : default;
    }
}