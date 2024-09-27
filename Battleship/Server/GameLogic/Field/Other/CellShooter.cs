using System.Numerics;
using Server.GameLogic.Field.Utils;

namespace Server.GameLogic.Field;

public static class CellShooter
{
    [Flags]
    public enum ShootResult
    {
        Missed = 0,
        HitTarget = 1,
        Destroy = 2,
        OutOfBounds = 4,
    }
    
    public static ShootResult Shoot(this Field field, Vector2 indexPosition)
    {
        var index = field.GetPositionIndex(indexPosition);

        if (index > field.Cells.GetUpperBound(0))
        {
            return ShootResult.OutOfBounds;
        }
        
        if (!field.Cells[index].HasFlag(Cell.Occupied))
        {
            return ShootResult.Missed;
        }
        
        return IsAllDestroyed(field, index) ? 
                 ShootResult.HitTarget|ShootResult.Destroy :
                 ShootResult.HitTarget;
    }

    private static bool IsAllDestroyed(Field field, int index)
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