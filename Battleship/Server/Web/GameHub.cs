using Microsoft.AspNetCore.SignalR;

namespace Server.Web;

public class GameHub : Hub
{
    #region Connection

    public override async Task OnConnectedAsync()
    {
        Console.WriteLine($"Connected {Context.ConnectionId}");
 
        GlobalLobby.ConnectPlayer(Context.ConnectionId);
        
        var connected = GlobalLobby.GetPlayer(Context.ConnectionId);
        connected.OnFieldUpdate += OnFieldUpdate;
        
        if (!GlobalLobby.TryAssignOpponent(Context.ConnectionId, out var opponentId))
        {
            return;
        }

        await CallLoadGame(opponentId);
        await CallLoadGame(Context.ConnectionId);
    }

    // TODO: при отключении противник должен побеждать???
    // как-то понимать что не дисконект (для MVC лишнее)
    public override Task OnDisconnectedAsync(Exception? exception)
    {
        var connected = GlobalLobby.GetPlayer(Context.ConnectionId);
        connected.OnFieldUpdate -= OnFieldUpdate;
        
        Console.WriteLine($"Disconnected {Context.ConnectionId}");
        
        GlobalLobby.DisconnectPlayer(Context.ConnectionId);

        return base.OnDisconnectedAsync(exception);
    }

    #endregion
    
    public async Task Hit(string index)
    {
        if (!int.TryParse(index, out int indexInt))
        {
            return;
        }

        GlobalLobby
            .GetPlayer(Context.ConnectionId)
            .Shoot(indexInt);
        
        await Task.CompletedTask;
    }

    private async Task CallLoadGame(string connectionId)
    { 
        await Clients
            .Client(connectionId)
            .SendAsync("LoadGame");
        
        await UpdateView(connectionId);
    }

    private void OnFieldUpdate()
    {
        _ = UpdateView(Context.ConnectionId);
    }
    
    private async Task UpdateView(string connectionId)
    {
        var player = GlobalLobby.GetPlayer(connectionId);
        
        await Clients
            .Client(connectionId)
            .SendAsync("UpdateView", 
                player.GetSelfFieldView(),
                player.GetEnemyFieldView());
    }
}