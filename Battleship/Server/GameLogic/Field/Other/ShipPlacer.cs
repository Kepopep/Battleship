using System.Numerics;
using Server.GameLogic.Field.Utils;

namespace Server.GameLogic.Field;

public class ShipPlacer(Field field)
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
        
        public ShipPosition(Ship.ShipType type, Vector2 position, bool vertical, int fieldSizeX)
        {
            Vertical = vertical;
            
            StartIndex = (int)position.X + (int)position.Y * fieldSizeX;

            var addSize = (byte)type - 1;
            EndIndex = StartIndex + (Vertical ? fieldSizeX * addSize : addSize);
            
            OccupyIndexes = new int[(int)type];
            
            for (int i = 0; i < (int)type; i++)
            {
                OccupyIndexes[i] = StartIndex + i * 
                    (Vertical ? fieldSizeX : 1);
            }
        }
    }
    
    public void PlaceShip(Ship.ShipType type, Vector2 indexPosition, bool isVertical)
    {
        var shipPosition = new ShipPosition(type, indexPosition, isVertical, field.SizeX);
        
        field.Occupy(shipPosition.OccupyIndexes);
    }
    
    public PlaceResult CanPlaceShip(Ship.ShipType type, Vector2 indexPosition, bool isVertical)
    {
        var positionToCheck = new ShipPosition(type, indexPosition, isVertical, field.SizeX);
        
        if (!InField(positionToCheck))
        {
            return PlaceResult.OutOfBounds;
        }

        if (IntersectOther(positionToCheck))
        {
            return PlaceResult.IntersectOther;
        }

        return PlaceResult.Success;
    }

    #region Validation
    
    private bool IntersectOther(ShipPosition positionToCheck)
    {
        var checkIndexes = GetNeighbors(positionToCheck);
        
        var lowerBound = field.Cells.GetLowerBound(0);
        var upperBound = field.Cells.GetUpperBound(0);
        
        foreach (var index in checkIndexes)
        {
            if (index < lowerBound || index > upperBound)
            {
                continue;
            }

            if (field.Cells[index] >= Cell.Occupied)
            {
                return true;
            }
        }
        
        return false;
    }

    private bool InField(ShipPosition positionToCheck)
    {
        var lowerBound = field.Cells.GetLowerBound(0);
        var upperBound = field.Cells.GetUpperBound(0);
        
        if (positionToCheck.StartIndex < lowerBound || 
            positionToCheck.StartIndex > upperBound || 
            (positionToCheck.Vertical ? 
                positionToCheck.EndIndex > upperBound : 
                positionToCheck.EndIndex % field.SizeY < positionToCheck.StartIndex % field.SizeX))
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
            result.AddRange(field.GetNeighbors(index));    
        }
        
        return result.
            Distinct().
            ToArray();
    }
    
    #endregion
}