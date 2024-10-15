using System.Runtime;

namespace Server.Web.Lobby.Game;

public interface IGameList
{
    public Task Add(GameFieldInfo gameFieldInfo);

    public Task Remove(Guid guid);
    
    public Task<GameFieldInfo> GetByOpponentId(string id);
    
    public Task<IEnumerable<GameFieldInfo>> GetAll();
}