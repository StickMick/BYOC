using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Json;
using BYOC.Console.Authorization;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.AspNetCore.WebUtilities;
using Serilog;

namespace BYOC.Console;

public class Client
{
    private readonly ILogger _logger;
    
    private HubConnection? hubConnection;
    private List<string> messages = new List<string>();
    private string? userInput;
    private string? messageInput;
    
    private JwtSecurityToken? _token;
    private string _tokenString;

    private HttpClientHandler _handler;

    public Client(ILogger logger)
    {
        _logger = logger;
        _handler = new HttpClientHandler();
        _handler.ClientCertificateOptions = ClientCertificateOption.Manual;
        _handler.ServerCertificateCustomValidationCallback = 
            (httpRequestMessage, cert, cetChain, policyErrors) =>
            {
                return true;
            };
    }

    private async Task<string> GetToken(string key)
    {
        if (_token == null || _token.ValidTo < DateTime.UtcNow)
        {
            await GetTokenFromServer(key);
        }
        else
        {
            _logger.Information("Existing token reused");    
        }
        return _tokenString;
    }

    private async Task<string> GetTokenFromServer(string key)
    {
        const string url = "https://localhost:7111/api/auth";
        var param = new Dictionary<string, string>()
        {
            { "apiKey", key }
        };

        var newUrl = new Uri(QueryHelpers.AddQueryString(url, param));
        
        var client = new HttpClient(_handler);
        
        var result = await client.GetFromJsonAsync<GetTokenRequest>(newUrl);

        if (string.IsNullOrEmpty(result?.JwtToken))
        {
            throw new Exception("Failed to get token");
        }

        _logger.Information("New token acquired");
        _tokenString = result.JwtToken;

        var handler = new JwtSecurityTokenHandler();
        _token = (handler.ReadToken(result.JwtToken) as JwtSecurityToken)!;
        return result.JwtToken;
    }
    
    public async Task StartAsync(CancellationToken token = default)
    {
        hubConnection = new HubConnectionBuilder()
            .WithUrl("https://localhost:7111/demo", (opts) =>
            {
                opts.AccessTokenProvider = async () => await GetToken("123456");

                opts.HttpMessageHandlerFactory = (message) =>
                {
                    if (message is HttpClientHandler clientHandler)
                        // always verify the SSL certificate
                        clientHandler.ServerCertificateCustomValidationCallback +=
                            (_, _, _, _) => true;
                    return message;
                };
            })
            .Build();

        hubConnection.On<string, string>("ReceiveMessage", (user, message) =>
        {
            _logger.Information("Received Message From {User}: {Message}", user, message);
        });

        await hubConnection.StartAsync(token);
    }

    public async Task SendAsync(string username, string message)
    {
        if (hubConnection is not null)
        {
            await hubConnection.SendAsync("SendMessage", username, message);
        }
    }

    public bool IsConnected =>
        hubConnection?.State == HubConnectionState.Connected;

    public async ValueTask DisposeAsync()
    {
        if (hubConnection is not null)
        {
            await hubConnection.DisposeAsync();
        }
    }
}