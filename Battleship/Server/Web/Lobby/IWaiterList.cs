namespace Server.Web.Lobby;

public interface IWaiterList
{
    public Task<IList<string>> GetAll();
    
    public Task<IList<string>> GetAllExcept(string waiterId);
    
    public Task Add(string waiterId);

    public Task Remove(string waiterId);
    
    public Task<bool> Contains(string waiterId);
}