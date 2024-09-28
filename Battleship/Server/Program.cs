﻿using Server.GameLogic;

namespace Server;

class Program
{
    static void Main(string[] args)
    {
        var gameLoop = new BattleShipGameLoop();
        gameLoop.Start();
        
#if DEBUG
        //gameLoop.Place(new Ship(ShipType.Single), 4, 5, false);
        //gameLoop.Place(new Ship(ShipType.Large), 1, 5, true);
        //gameLoop.Place(new Ship(ShipType.Middle), 10, 8, false);
        gameLoop.Display();
#endif
        
        Console.WriteLine($"Server started");
        
        Console.WriteLine("Press any key to stop the server...");
        Console.ReadKey();
    }
}
