namespace Server.Web.Lobby;

public struct WaiterListPair
{
    public WaiterListPair() { }

    public enum Status
    {
        Alone,
        HasPair
    }
    
    public Status PairStatus { get; set; } = Status.Alone;
    public string PairId { get; set; } = string.Empty;
}