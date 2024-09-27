namespace Server.GameLogic.Field;

public class Field
{
    public Cell[] Cells { get; private set; }
    
    public List<List<int>> ShipCells { get; private set; }

    public readonly byte SizeX;

    public readonly byte SizeY; 
    
    public Field(byte sizeX, byte sizeY)
    {
        Cells = new Cell[10*10];
        ShipCells = new List<List<int>>();
        
        SizeX = sizeX;
        SizeY = sizeY;
    }


    #region Occupy

    public void SetShipIndexes(IList<int> indexes)
    {
        foreach (var index in indexes)
        {
            Cells[index] = Cell.Occupied;
        }
        
        ShipCells.Add(indexes.ToList());
    }

    #endregion
    
    #region Shoot

    public void Shoot(int index)
    {
        Cells[index] |= Cell.Attacked;
    }
    
    #endregion
}