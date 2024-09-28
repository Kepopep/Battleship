using System.Text;
using Server.GameLogic.Field;

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