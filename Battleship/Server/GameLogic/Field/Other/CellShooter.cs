using System.Numerics;
using Server.GameLogic.Field.Utils;

namespace Server.GameLogic.Field;

public class CellShooter
{
    [Flags]
    public enum ShootResult
    {
        Missed = 0,
        HitTarget = 1,
        Destroy = 2,
        OutOfBounds = 4,
    }
    
    private Field _field;
    
    public CellShooter(Field field)
    {
        _field = field;
    }
    
    public ShootResult Shoot(Vector2 indexPosition)
    {
        var index = _field.GetPositionIndex(indexPosition);

        if (index > _field.Cells.GetUpperBound(0))
        {
            return ShootResult.OutOfBounds;
        }
        
        if (!_field.Cells[index].HasFlag(Cell.Occupied))
        {
            return ShootResult.Missed;
        }
        
        _field.Shoot(index);
        
        return IsAllDestroyed(_field, index) ? 
                 ShootResult.HitTarget|ShootResult.Destroy :
                 ShootResult.HitTarget;
    }

    private bool IsAllDestroyed(Field field, int index)
    {
        var relatedShip = field.ShipCells.Find(x => x.Contains(index));

        if (relatedShip == null)
        {
            return false;
        }

        return relatedShip
            .Where(x => x != index)
            .All(otherIndex => field.Cells[otherIndex]
                .HasFlag(Cell.Attacked));
    }
}