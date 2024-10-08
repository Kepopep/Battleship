namespace Server.GameLogic.Field;

public class CellShooter
{
    public bool HasTarget => _field != null;
    
    private Field? _field;

    [Flags]
    public enum ShootResult
    {
        OutOfBounds = 0,
        Missed = 1,
        HitTarget = 2,
        NoTarget = 4,
    }

    public void InitField(Field? field)
    {
        _field = field;
    }

    public ShootResult Shoot(int index)
    {
        if (_field == null)
        {
            return ShootResult.NoTarget;
        }
        
        if (index > _field.Cells.GetUpperBound(0))
        {
            return ShootResult.OutOfBounds;
        }
        
        _field.Attack(index);
        
        return !_field.Cells[index].HasFlag(Cell.Occupied) ? 
            ShootResult.Missed : 
            ShootResult.HitTarget;
    }
}