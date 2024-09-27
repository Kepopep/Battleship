namespace Server.GameLogic.Field;

[Flags]
public enum Cell
{
    Empty = 0,
    Occupied = 1,
    Attacked = 2
}