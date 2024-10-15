using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.SignalR;
using Server.GameLogic.Ship;
using Server.Web.Hub.DTO;

namespace Server.Web.Hub;

public class GameHub : Hub<IGameHub>
{
    private static readonly HttpClient Client = new HttpClient();
    
    #region Connection

    public override async Task OnConnectedAsync()
    {
        Console.WriteLine($"Client {Context.ConnectionId} connected");
        await Client.PostAsync($"http://localhost:5000/api/wait-lobby/connect/{Context.ConnectionId}", new StringContent(""));
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        Console.WriteLine($"Client {Context.ConnectionId} disconnected");
        await Client.PostAsync(
            $"http://localhost:5000/api/wait-lobby/disconnect/{Context.ConnectionId}", 
            new StringContent("")
            );
        
        await base.OnDisconnectedAsync(exception);
    }

    #endregion

    public async Task Place(object info)
    {
        if (info is not JsonElement jsonInfo)
        {
            return;
        }

        try
        {
            var postInfo = new PlacePostInfo
            {
                ConnectionId = Context.ConnectionId,
                CellIndex = jsonInfo
                    .GetProperty("CellIndex")
                    .GetInt32(),
                Type = (ShipType)jsonInfo
                    .GetProperty("Type")
                    .GetInt32(),
                IsVertical = jsonInfo
                    .GetProperty("IsVertical")
                    .GetBoolean()
            };
            
            await Client.PostAsync(
                $"http://localhost:5000/api/game-lobby/place",
                JsonContent.Create(postInfo)
            );
        }
        catch (Exception e)
        {
            await Console.Error.WriteLineAsync($"Validation fail, GameHub.Place \n {e.Message}");
            throw;
        }
        
        await Task.CompletedTask;
    }
    
    public async Task Hit(string index)
    {
        if (!int.TryParse(index, out int indexInt))
        {
            return;
        }

        await Task.CompletedTask;
    }
}