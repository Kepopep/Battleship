using System.Numerics;
using System.Text;
using Server.GameLogic.Field;
using Server.GameLogic.Ship;

namespace Server.GameLogic;

public class BattleShipGameLoop
{
    private readonly Field.Field _field;
    
    private readonly ShipPlacer _placer;
    
    private readonly CellShooter _shooter;

    public BattleShipGameLoop()
    {
        _field = new Field.Field(10, 10);
        
        _placer = new ShipPlacer(_field);
        _shooter = new CellShooter(_field);
    }
    
    public void Start()
    {
        
    }

    public void End()
    {
        
    }

    public void Place(Ship.Ship ship, int x, int y, bool vertical)
    {
        var result = _placer.PlaceShip(ship, new Vector2(x-1, y-1), vertical);
        
#if DEBUG
        Console.WriteLine($"Place end: {ship} {result}");
#endif
    }

    public void Shoot(int x, int y)
    {
        var result = _shooter.Shoot(new Vector2(x - 1, y - 1));
        
        if(result.HasFlag(CellShooter.ShootResult.Destroy))
        {
            
#if DEBUG
        Console.WriteLine($"Ship destroyed {(CheckWin() ? "win" : "")}");
#endif
        }
    }

    private bool CheckWin()
    {
        return _field.ShipCells
            .TrueForAll(x => 
                x.TrueForAll(y => 
                    _field.Cells[y].HasFlag(Cell.Attacked)));
    }

#if DEBUG
    public void Display()
    {
        var stringBuilder = new StringBuilder();

        var displaySymbols = new Dictionary<Cell, char>()
        {
            { Cell.Attacked , 'X'},
            { Cell.Occupied , 'O'},
            { Cell.Empty , '+'},
            { (Cell)3 , 'W'}
        };
        
        for (int i = 0; i < _field.SizeY; i++)
        {
            for (int j = 0; j < _field.SizeX; j++)
            {
                stringBuilder.Append($"{displaySymbols[_field.Cells[i * _field.SizeX + j]]} ");
            }    
            stringBuilder.Append('\n');
            
        }
        
        Console.WriteLine(stringBuilder.ToString());
    }
#endif
}