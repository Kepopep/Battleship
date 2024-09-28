using System.Numerics;

namespace Server.GameLogic.Field;

public class Field
{
    public event Action<int, Cell> OnCellStateChange;
    
    public readonly Cell[] Cells;

    public readonly byte SizeX;

    public readonly byte SizeY; 
    
    public Field(byte sizeX, byte sizeY)
    {
        Cells = new Cell[sizeX*sizeY];
        
        SizeX = sizeX;
        SizeY = sizeY;
    }

    public void Occupy(IList<int> indexes)
    {
        foreach (var index in indexes)
        {
            SetCell(index, Cell.Occupied);
        }
    }

    public void Attack(int index)
    {
        UpdateCell(index, Cell.Attacked);
    }
    
    #region Cell
    
    protected virtual void SetCell(int index, Cell cell)
    {
        Cells[index] = cell;
        OnCellStateChange?.Invoke(index, cell);
    }
    
    protected virtual void UpdateCell(int index, Cell cell)
    {
        Cells[index] |= cell;
        OnCellStateChange?.Invoke(index, cell);
    }
    
    #endregion
    
    public int Position2Index(Vector2 position)
    {
        return (int)position.X + (int)position.Y * SizeX;
    }
}