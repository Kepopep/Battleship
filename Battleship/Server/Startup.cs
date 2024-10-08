using Server.Web;

namespace Server;

internal class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddSignalR();
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

        app.UseEndpoints(endpoint =>
        {
            endpoint.MapHub<GameHub>("/hub");
        });
    }
}