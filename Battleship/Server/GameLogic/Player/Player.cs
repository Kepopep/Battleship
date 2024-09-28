using System.Numerics;
using Server.GameLogic.Field;
using Server.GameLogic.Ship;

namespace Server.GameLogic.Player;

public class Player
{
    public event Action<Player> OnFail; 
    
    private readonly Field.Field _field;   
    
    private readonly ShipPlacer _shipPlacer;
    
    private readonly CellShooter _cellShooter;

    private List<Ship.Ship> _ships;

    public Player()
    {
        _field = new Field.Field(10, 10);
        _field.OnCellStateChange += OnCellStateChange;
        
        _shipPlacer = new ShipPlacer(_field);
        _cellShooter = new CellShooter(_field);

        _ships = new List<Ship.Ship>();
    }

    // TODO передача позиций кораблей
    // тут уже все сохраняется
    public void PlaceShips()
    {
        _shipPlacer.PlaceShip(ShipType.Big, new Vector2(0, 0), true);
        
    }

    // TODO очистка клеток
    public void RemoveShip(Ship.Ship ship)
    {
        
    }

    public void Shoot(Vector2 position)
    {
        _cellShooter.Shoot(position);
    }

    #region Handlers
    
    private void OnCellStateChange(int index, Cell cell)
    {
        if (!cell.HasFlag(Cell.Occupied | Cell.Attacked))
        {
            return;
        }
        
        if (_ships.TrueForAll(x => x.IsDestroyed))
        {
            OnFail?.Invoke(this);   
        }
    }
    
    #endregion
}