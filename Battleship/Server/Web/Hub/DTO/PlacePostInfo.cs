using Server.GameLogic.Ship;

namespace Server.Web.Hub.DTO;

public class PlacePostInfo
{
    public string ConnectionId { get; set; }
    
    public int CellIndex { get; set; }
    
    public ShipType Type { get; set; }
    
    public bool IsVertical { get; set; }
}