using Server.GameLogic.Field;

namespace Server.GameLogic.Ship;

public class Ship
{
    public event Action<Ship> OnDestroyed;
    
    private readonly Field.Field _field;
    
    private readonly List<int> _placeCells;
    
    public ShipType Type { get; private set; }

    public bool IsDestroyed => _placeCells.TrueForAll(x => _field.Cells[x].HasFlag(Cell.Attacked));
    

    public Ship(ShipType shipType, Field.Field field, IList<int> placeCells)
    {
        _field = field;
        _field.OnCellStateChange += OnCellStateChange;
        
        _placeCells = placeCells.ToList();
        
        Type = shipType;
    }
    
    #region Handlers
    
    private void OnCellStateChange(int index, Cell state)
    {
        if (!_placeCells.Contains(index))
        {
            return;
        }

        if (IsDestroyed)
        {
            OnDestroyed?.Invoke(this);
        }
    }
    
    #endregion
}