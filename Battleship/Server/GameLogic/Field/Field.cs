namespace Server.GameLogic.Field;

public class Field
{
    public Cell[] Cells { get; private set; }

    public readonly byte SizeX;

    public readonly byte SizeY; 
    
    public Field(byte sizeX, byte sizeY)
    {
        Cells = new Cell[10*10];
        
        SizeX = sizeX;
        SizeY = sizeY;
    }


    #region Occupy

    public void OccupyCells(IList<int> indexes)
    {
        foreach (var index in indexes)
        {
            OccupyCell(index);
        }
    }
    
    public void OccupyCell(int index)
    {
        Cells[index] = Cell.Occupied;
    }

    #endregion
    
    #region Shoot

    public void Shoot(int index)
    {
        Cells[index] |= Cell.Attacked;
    }
    
    #endregion
}