using System.Numerics;
using Server.GameLogic.Field;
using Server.GameLogic.Ship;

namespace Server.GameLogic.Player;

public class Player
{
    private readonly Field.Field _field;   
    
    private readonly ShipPlacer _shipPlacer;
    
    private readonly CellShooter _cellShooter;

    private Player _enemy;
    
    public Player(IConfiguration configuration)
    {
        var fieldConfigSection = configuration
            .GetSection("FieldConfig");
            
        _field = new Field.Field(
            fieldConfigSection
                .GetSection("SizeX")
                .Get<byte>(),
            fieldConfigSection
                .GetSection("SizeX")
                .Get<byte>());
        
        _shipPlacer = new ShipPlacer(_field, configuration);
        _cellShooter = new CellShooter();
    }

    public void AssignEnemy(Player enemy)
    {
        _enemy = enemy;
        _cellShooter.InitField(_enemy._field);
    }
    
    public void Shoot(int index) 
    {
        switch (_cellShooter.Shoot(index))
        {
            case CellShooter.ShootResult.OutOfBounds:
                Console.WriteLine($"Hit: OutOfBounds");
                break;
            case CellShooter.ShootResult.Missed:
                Console.WriteLine($"Hit: Miss");
                break;
            case CellShooter.ShootResult.HitTarget:
                Console.WriteLine($"Hit: Hit");
                break;
            case CellShooter.ShootResult.NoTarget:
                Console.WriteLine($"Hit: NoTarget");
                break;
            default:
                return;
        }
    }

    public void Place(int index, ShipType type, bool isVertical)
    {
        switch (_shipPlacer.CanPlaceShip(type, index, isVertical))
        {
            case ShipPlacer.PlaceResult.Success:
                Console.WriteLine($"Place: Success");
                _shipPlacer.PlaceShip(type, index, isVertical);
                break;
            case ShipPlacer.PlaceResult.OutOfBounds:
                Console.WriteLine($"Place: OutOfBounds");
                break;
            case ShipPlacer.PlaceResult.IntersectOther:
                Console.WriteLine($"Place: IntersectOther");
                break;
            case ShipPlacer.PlaceResult.TypeMaxCount:
                Console.WriteLine($"Place: TypeMaxCount {type}");
                break;
            default:
                break;
        }
    }

    #region Field representation
    
    public Cell[] GetSelfFieldView()
    {
        return _field.Cells;
    }

    public Cell[] GetOpponentFieldView()
    {
        return _enemy._field
            .Cells
            .Select(x => 
                x.HasFlag(Cell.Attacked) ? 
                    x : 
                    x & ~Cell.Occupied)
            .ToArray();
    }
    
    #endregion
}