using Server.Web.Misc;

namespace Server.Web.Lobby;

public class GamesMemoryList : IGameList
{
    private static readonly List<GameFieldInfo> Games = new List<GameFieldInfo>();
    
    public Task Add(GameFieldInfo opponentInfo)
    {
        Games.Add(opponentInfo);
        return Task.CompletedTask;
    }

    public Task Remove(Guid guid)
    {
        var removeElement = Games.Find(x => x.Id == guid);

        if (!removeElement.Equals(default(GameFieldInfo)))
        {
            Games.Remove(removeElement);
        }
        return Task.CompletedTask;
    }

    public Task<GameFieldInfo> GetByOpponentId(string id)
    {
        return Task.FromResult(Games.Find(x => x.HasId(id)));
    }

    public Task<IEnumerable<GameFieldInfo>> GetAll()
    {
        return Task.FromResult(Games.AsEnumerable());
    }
}