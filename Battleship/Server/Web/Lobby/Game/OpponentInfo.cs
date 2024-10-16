using Server.GameLogic.Player;

namespace Server.Web.Lobby;

public struct OpponentInfo
{
    public Guid Id { get; set; }
    
    public string ConnectionId { get; set; }
    
    public Player? Player { get; set; }
}