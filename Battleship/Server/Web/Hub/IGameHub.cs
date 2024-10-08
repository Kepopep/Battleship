namespace Server.Web.Hub;

public interface IGameHub
{
    public Task LoadGame(string opponentId);
}