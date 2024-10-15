using Server.GameLogic.Ship;

namespace Server.Web.Hub.DTO;

public class ShootPostInfo
{
    public string ConnectionId { get; set; }
    
    public int CellIndex { get; set; }
}