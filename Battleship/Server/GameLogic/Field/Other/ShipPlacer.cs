using Server.GameLogic.Field.Utils;
using Server.GameLogic.Ship;

namespace Server.GameLogic.Field;

public class ShipPlacer
{
    private ShipTypeCounter _typeCounter;
    
    private readonly Field _field;

    public ShipPlacer(Field field, IConfiguration configuration)
    {
        _field = field;
        _typeCounter = new ShipTypeCounter(configuration
            .GetSection("ShipConfig")
            .Get<ShipConfig>());
    }

    public enum PlaceResult
    {
        Success,
        OutOfBounds,
        IntersectOther,
        TypeMaxCount
    }
    
    private struct ShipPosition
    {
        public readonly int StartIndex; 
        
        public readonly int EndIndex; 
        
        public readonly bool Vertical;

        public readonly int[] OccupyIndexes;
        
        public ShipPosition(Ship.ShipType type, int index, bool vertical, int fieldSizeX)
        {
            Vertical = vertical;
            
            StartIndex = index;

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
    
    public void PlaceShip(Ship.ShipType type, int index, bool isVertical)
    {
        var shipPosition = new ShipPosition(type, index, isVertical, _field.SizeX);
        
        _field.Occupy(shipPosition.OccupyIndexes);
        
        _typeCounter.Add(type);
    }
    
    public PlaceResult CanPlaceShip(Ship.ShipType type, int index, bool isVertical)
    {
        var positionToCheck = new ShipPosition(type, index, isVertical, _field.SizeX);

        if (!_typeCounter.CanAdd(type))
        {
            return PlaceResult.TypeMaxCount;
        }
        
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
    
    #endregion
}