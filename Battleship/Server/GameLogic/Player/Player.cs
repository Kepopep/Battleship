using System.Numerics;
using Server.GameLogic.Field;
using Server.GameLogic.Ship;

namespace Server.GameLogic.Player;

public class Player
{
    public event Action OnFail; 
    
    public event Action OnFieldUpdate; 
    
    private readonly Field.Field _field;   
    
    private readonly ShipPlacer _shipPlacer;
    
    private readonly CellShooter _cellShooter;
    
    private List<Ship.Ship> _ships;

    private Player _enemy;

    public bool HasEnemy => _cellShooter.HasTarget;

    public Player()
    {
        _field = new Field.Field(10, 10);
        _field.OnCellStateChange += OnCellStateChange;
        
        _shipPlacer = new ShipPlacer(_field);
        _cellShooter = new CellShooter();

        _ships = new List<Ship.Ship>();
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

    #region Handlers
    
    private void OnCellStateChange(int index, Cell cell)
    {
        OnFieldUpdate?.Invoke();
        
        if (!cell.HasFlag(Cell.Occupied | Cell.Attacked))
        {
            return;
        }
        
        if (_ships.TrueForAll(x => x.IsDestroyed))
        {
            OnFail?.Invoke();   
        }
    }
    
    #endregion

    #region Field representation
    
    public Cell[] GetSelfFieldView()
    {
        return _field.Cells;
    }

    public Cell[] GetEnemyFieldView()
    {
        return _enemy._field
            .Cells
            .Select(x => x & ~Cell.Occupied)
            .ToArray();
    }
    
    #endregion
}