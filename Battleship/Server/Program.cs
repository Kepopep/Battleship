using Server.GameLogic;
using Server.GameLogic.Ship;

namespace Server;

using WebSocketSharp.Server;

using Behaviors;

// документация: http://sta.github.io/websocket-sharp/

class Program
{
    static void Main(string[] args)
    {
        var server = new WebSocketServer("ws://localhost:8000");
        
        server.AddWebSocketService<BaseBehavior>("/Base");
        
        server.Start();

        var gameLoop = new BattleShipGameLoop();
        gameLoop.Start();
        
#if DEBUG
        gameLoop.Place(new Ship(ShipType.Single), 4, 5, false);
        gameLoop.Place(new Ship(ShipType.Large), 1, 5, true);
        gameLoop.Place(new Ship(ShipType.Middle), 10, 8, false);
        gameLoop.Display();
#endif
        
        Console.WriteLine($"Server started");
        
        Console.WriteLine("Press any key to stop the server...");
        Console.ReadKey();
        
        server.Stop();
    }
}
