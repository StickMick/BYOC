using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.SignalR.Client;
using Serilog;

namespace BYOC.ConsoleClient.SignalrClient;

public partial class Client
{
    private readonly ILogger _logger;
    private HttpClientHandler _handler;
    
    private HubConnection? _hubConnection;
    
    private JwtSecurityToken? _token;
    private string? _tokenString;

    

    public Client(ILogger logger)
    {
        _logger = logger;
        _handler = new HttpClientHandler();
        _handler.ClientCertificateOptions = ClientCertificateOption.Manual;
        _handler.ServerCertificateCustomValidationCallback = 
            (_, _, _, _) => true;
    }
    
}