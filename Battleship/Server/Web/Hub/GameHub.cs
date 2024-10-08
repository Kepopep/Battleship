using Microsoft.AspNetCore.SignalR;

namespace Server.Web.Hub;

public class GameHub : Hub<IGameHub>
{
    #region Connection

    public override async Task OnConnectedAsync()
    {
        // вызов LobbyController??
    }

    public override Task OnDisconnectedAsync(Exception? exception)
    {
        // вызов LobbyController??
        return base.OnDisconnectedAsync(exception);
    }

    #endregion
    
    public async Task Hit(string index)
    {
        if (!int.TryParse(index, out int indexInt))
        {
            return;
        }

        await Task.CompletedTask;
    }
}