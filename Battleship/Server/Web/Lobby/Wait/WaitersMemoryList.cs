namespace Server.Web.Lobby;

public class WaitersMemoryList : IWaiterList
{
    private static readonly List<Waiter> Waiters = new List<Waiter>();

    public Task<IEnumerable<string>> GetAll()
    {
        IEnumerable<string> waitersId = Waiters
            .Select(x => x.Id);

        return Task.FromResult(waitersId);
    }
    
    public Task Add(string waiterId)
    {
        Waiters.Add(new Waiter(waiterId));
        return Task.CompletedTask;
    }

    public Task Remove(string waiterId)
    {
        var removeElement = Waiters.Find(x => x.Id == waiterId);

        if (removeElement != null)
        {
            Waiters.Remove(removeElement);
        }

        return Task.CompletedTask;
    }
}