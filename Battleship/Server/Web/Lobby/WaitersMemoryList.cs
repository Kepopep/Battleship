namespace Server.Web.Lobby;

public class WaitersMemoryList : IWaiterList
{
    private static readonly List<Waiter> Waiters = new List<Waiter>();

    public Task<IList<string>> GetAll()
    {
        IList<string> waitersId = Waiters
            .Select(x => x.Id)
            .ToList();

        return Task.FromResult(waitersId);
    }

    public Task<IList<string>> GetAllExcept(string waiterId)
    {
        IList<string> waitersId = Waiters
            .Where(x => x.Id != waiterId)
            .Select(x => x.Id)
            .ToList();

        return Task.FromResult(waitersId);
    }

    public Task Add(string waiterId)
    {
        Console.WriteLine($"Connected {waiterId}");
        
        Waiters.Add(new Waiter(waiterId));
        return Task.CompletedTask;
    }

    public Task Remove(string waiterId)
    {
        Console.WriteLine($"Disconnected {waiterId}");
        
        var removeElement = Waiters.Find(x => x.Id == waiterId);

        if (removeElement != null)
        {
            Waiters.Remove(removeElement);
        }

        return Task.CompletedTask;
    }

    public Task<bool> Contains(string waiterId)
    {
        return Task.FromResult(Waiters.Any(x => x.Id == waiterId));
    }

}