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

        Console.WriteLine($"Server started");
        
        Console.WriteLine("Press any key to stop the server...");
        Console.ReadKey();
        
        server.Stop();
    }
}
