using System.Numerics;
using System.Text;
using Server.GameLogic.Ship;

namespace Server.GameLogic;

public class BattleShipGameLoop
{
    private readonly Field.Field _field;
    
    private readonly ShipPlacer _placer;

    public BattleShipGameLoop()
    {
        _field = new Field.Field(10, 10);
        _placer = new ShipPlacer(_field);
    }
    
    public void Start()
    {
        
    }

    public void End()
    {
        
    }

    public void Place(Ship.Ship ship, int x, int y, bool vertical)
    {
        var result = _placer.PlaceShip(ship, new Vector2(x - 1, y -1), vertical);
        
#if DEBUG
        Console.WriteLine($"Place end: {ship} {result}");
#endif
    }

#if DEBUG
    public void Display()
    {
        var stringBuilder = new StringBuilder();
        
        for (int i = 0; i < _field.SizeY; i++)
        {
            for (int j = 0; j < _field.SizeX; j++)
            {
                stringBuilder.Append($"{_field.Cells[i * _field.SizeX + j]} ");
            }    
            stringBuilder.Append('\n');
            
        }
        
        Console.WriteLine(stringBuilder.ToString());
    }
#endif
}