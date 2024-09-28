using System.Numerics;
using Server.GameLogic.Field.Utils;

namespace Server.GameLogic.Field;

public class CellShooter(Field field)
{
    [Flags]
    public enum ShootResult
    {
        OutOfBounds = 0,
        Missed = 1,
        HitTarget = 2
    }

    public ShootResult Shoot(Vector2 indexPosition)
    {
        var index = field.Position2Index(indexPosition);

        if (index > field.Cells.GetUpperBound(0))
        {
            return ShootResult.OutOfBounds;
        }
        
        if (!field.Cells[index].HasFlag(Cell.Occupied))
        {
            return ShootResult.Missed;
        }
        
        field.Attack(index);
        
        return ShootResult.HitTarget;
    }
}