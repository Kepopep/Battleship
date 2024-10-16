using Server.Web.Hub;
using Server.Web.Lobby;
using Server.Web.Lobby.Game;

namespace Server;

internal class Startup
{
    public IConfiguration Configuration { get; set; }
    
    public Startup(IConfiguration configuration)
    {
        var builder = new ConfigurationBuilder()
            .AddJsonFile("Config/conf.json", optional: false, reloadOnChange: true);
        Configuration = builder.Build();
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers();

        services.AddSingleton<IWaiterList, WaitersMemoryList>();
        services.AddSingleton<IGameList, GamesMemoryList>();
        
        services.AddSingleton<IConfiguration>(Configuration);
        
        services.AddSignalR();
        services.AddCors(options =>
        {
            options.AddDefaultPolicy(
                builder =>
                {
                    builder.WithOrigins("http://localhost:5000")
                        .AllowAnyHeader()
                        .WithMethods("GET", "POST")
                        .AllowCredentials();
                });
        });
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        var webSocketOptions = new WebSocketOptions()
        {
            KeepAliveInterval = TimeSpan.FromMinutes(1),
        };

        app.UseWebSockets(webSocketOptions);

        app.UseDefaultFiles();
        app.UseStaticFiles();
        
        app.UseRouting();

        app.UseCors();

        app.UseEndpoints(endpoint =>
        {
            endpoint.MapControllers();
            endpoint.MapHub<GameHub>("/hub");
        });
    }
}