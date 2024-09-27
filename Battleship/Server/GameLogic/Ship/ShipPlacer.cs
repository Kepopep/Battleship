using System.Numerics;
using Server.GameLogic.Field;
using Server.GameLogic.Field.Utils;


namespace Server.GameLogic.Ship;

public class ShipPlacer
{
    public enum PlaceResult
    {
        Success,
        OutOfBounds,
        IntersectOther,
    }
    
    private struct ShipPosition
    {
        public readonly int StartIndex; 
        
        public readonly int EndIndex; 
        
        public readonly bool Vertical;

        public readonly int[] OccupyIndexes;
        
        public ShipPosition(Ship ship, Vector2 position, bool vertical, int fieldSizeX)
        {
            Vertical = vertical;
            
            StartIndex = (int)position.X + (int)position.Y * fieldSizeX;

            var addSize = (byte)ship.Type - 1;
            EndIndex = StartIndex + (Vertical ? fieldSizeX * addSize : addSize);
            
            OccupyIndexes = new int[(int)ship.Type];
            
            for (int i = 0; i < (int)ship.Type; i++)
            {
                OccupyIndexes[i] = StartIndex + i * 
                    (Vertical ? fieldSizeX : 1);
            }
        }
    }
    
    private readonly Field.Field _field;

    public ShipPlacer(Field.Field field)
    {
        _field = field;
    }
    
    public PlaceResult PlaceShip(Ship ship, Vector2 indexPosition, bool isVertical)
    {
        var positionToCheck = new ShipPosition(ship, indexPosition, isVertical, _field.SizeX);
        
        if (!InField(positionToCheck))
        {
            return PlaceResult.OutOfBounds;
        }

        if (IntersectOther(positionToCheck))
        {
            return PlaceResult.IntersectOther;
        }

        _field.OccupyCells(positionToCheck.OccupyIndexes);
        
        return PlaceResult.Success;
    }

    private bool IntersectOther(ShipPosition positionToCheck)
    {
        var checkIndexes = GetNeighbors(positionToCheck);
        
        var lowerBound = _field.Cells.GetLowerBound(0);
        var upperBound = _field.Cells.GetUpperBound(0);
        
        foreach (var index in checkIndexes)
        {
            if (index < lowerBound || index > upperBound)
            {
                continue;
            }

            if (_field.Cells[index] >= Cell.Occupied)
            {
                return true;
            }
        }
        
        return false;
    }

    private bool InField(ShipPosition positionToCheck)
    {
        var lowerBound = _field.Cells.GetLowerBound(0);
        var upperBound = _field.Cells.GetUpperBound(0);
        
        if (positionToCheck.StartIndex < lowerBound || 
            positionToCheck.StartIndex > upperBound || 
            (positionToCheck.Vertical ? 
                positionToCheck.EndIndex > upperBound : 
                positionToCheck.EndIndex % _field.SizeY < positionToCheck.StartIndex % _field.SizeX))
        {
            // Начало или конец карабля находится за пределами поля
            return false;
        }
        
        return true;
    }

    private int[] GetNeighbors(ShipPosition positionToCheck)
    {
        var result = new List<int>();
        
        foreach (var index in positionToCheck.OccupyIndexes)
        {
            result.AddRange(_field.GetNeighbors(index));    
        }
        
        return result.
            Distinct().
            ToArray();
    }
    
}