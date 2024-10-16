namespace Server.GameLogic.Ship;

public class ShipConfig
{
    public TypeInfo[] Configs { get; set; } 
}
    
public class TypeInfo
{
    public ShipType ShipType { get; set; }
    
    public int MaxCount { get; set; }
}