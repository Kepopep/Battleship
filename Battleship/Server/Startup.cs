using Server.Web;
using Server.Web.Hub;

namespace Server;

internal class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddSignalR();
        services.AddControllers();
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
            //endpoint.MapControllers();
            endpoint.MapHub<GameHub>("/hub");
        });
    }
}