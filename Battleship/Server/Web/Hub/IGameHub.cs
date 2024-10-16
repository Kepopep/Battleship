using Server.Web.Misc;

namespace Server.Web.Hub;

public interface IGameHub
{
    public Task LoadGame();
    
    public Task UpdateSelfField(GameFieldView view);
}