using Server.GameLogic;

namespace Server;

class Program
{
    static void Main(string[] args)
    {
        CreateHostBuilder().Build().Run();
    }

    public static IHostBuilder CreateHostBuilder()
    {
        return Host.CreateDefaultBuilder().ConfigureWebHostDefaults(webHost =>
        { 
            webHost.UseStartup<Startup>();
        });
    }
}