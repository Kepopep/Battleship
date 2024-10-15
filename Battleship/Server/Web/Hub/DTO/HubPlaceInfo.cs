using Server.GameLogic.Ship;

namespace Server.Web.Hub.DTO;

public class HubPlaceInfo
{
    public int CellIndex { get; set; }
    
    public ShipType Type { get; set; }

    public bool IsVertical { get; set; }
}