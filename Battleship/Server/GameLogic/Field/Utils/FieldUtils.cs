using System.Numerics;

namespace Server.GameLogic.Field.Utils;

public static class FieldUtils
{ 
    private static readonly int[,] Directions =
        new int[8, 2]
        {
            {-1, 0 }, { 1, 0}, // вверх, вниз
            { 0, -1}, { 0, 1}, // влево, вправо
            {-1, -1}, {-1, 1}, // вверх-влево, вверх-вправо
            { 1, -1}, { 1, 1}  // вниз-влево, вниз-вправо 
        };

    public static List<int> GetNeighbors(this Field field, int index)
    {
        var result = new List<int>();
        
        var x = index % field.SizeX;
        var y = index / field.SizeX;

        for (var i = 0; i < Directions.GetUpperBound(0); i++)
        {
            var newX = x + Directions[i,1];
            var newY = y + Directions[i,0];

            if (newY < 0 || newY >= field.SizeY ||
                newX < 0 || newX >= field.SizeX)
            {
                continue;
            }
            
            result.Add(newX + newY * field.SizeX);
        }
        
        return result;
    }
}