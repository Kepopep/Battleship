namespace Server.GameLogic.Ship;

public class Ship
{
    private ShipType _shipType;
    
    public ShipType Type => _shipType;

    public Ship(ShipType shipType)
    {
        _shipType = shipType;
    }
}