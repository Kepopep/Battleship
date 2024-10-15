namespace Server.Web.Lobby;

public interface IWaiterList
{
    public Task Add(string waiterId);

    public Task Remove(string waiterId);
    
    public Task<IEnumerable<string>> GetAll();
}