using Microsoft.AspNetCore.SignalR;
using Server.Web.Hub;

namespace Server.Web.Lobby.Game;

public static class LobbyHelper
{
    public static async Task ToWaitGroup(this IHubContext<GameHub, IGameHub> hub, string id)
    {
        await hub.Groups.RemoveFromGroupAsync(id, "game-lobby");
        await hub.Groups.AddToGroupAsync(id, "wait-lobby");
    }
    
    public static async Task ToGameGroup(this IHubContext<GameHub, IGameHub> hub, string id, string gameId)
    {
        await hub.Groups.RemoveFromGroupAsync(id, "wait-lobby");
        await hub.Groups.AddToGroupAsync(id, gameId);
    }
}