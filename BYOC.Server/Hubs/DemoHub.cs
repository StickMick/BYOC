using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using ILogger = Serilog.ILogger;

namespace BYOC.Server.Hubs;

public class DemoHub : Hub
{
    private readonly ILogger _logger;

    public DemoHub(ILogger logger)
    {
        _logger = logger;
    }
    
    [Authorize(AuthenticationSchemes = "Bearer")]
    public async Task SendMessage(string user, string message)
    {
        _logger.Information("Message received from user: {user}", user);
        await Clients.All.SendAsync("ReceiveMessage", user, message);
    }
}