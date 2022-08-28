using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Json;
using BYOC.ConsoleClient.RequestModels;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.AspNetCore.WebUtilities;

namespace BYOC.ConsoleClient.SignalrClient;

public partial class Client
{
    private async Task<string> GetToken(string key)
    {
        if (_token == null || _tokenString == null || _token.ValidTo < DateTime.UtcNow)
        {
            _tokenString = await GetTokenStringFromServer(key);
            _token = (new JwtSecurityTokenHandler().ReadToken(_tokenString) as JwtSecurityToken)!;
        }
        else
        {
            _logger.Information("Existing token reused");
        }

        return _tokenString;
    }

    private async Task<string> GetTokenStringFromServer(string key)
    {
        const string url = "https://localhost:7111/api/auth";
        var param = new Dictionary<string, string?>()
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
        
        return result.JwtToken;
    }
    
    public async Task StartAsync(string apiKey, CancellationToken token = default)
    {
        _hubConnection = new HubConnectionBuilder()
            .WithUrl("https://localhost:7111/game", (opts) =>
            {
                opts.AccessTokenProvider = async () => await GetToken(apiKey);

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

        BindResponseCallbacks();

        await _hubConnection.StartAsync(token);
    }

    public bool IsConnected =>
        _hubConnection?.State == HubConnectionState.Connected;

    public async ValueTask DisposeAsync()
    {
        if (_hubConnection is not null)
        {
            await _hubConnection.DisposeAsync();
        }
    }

    public async Task Disconnect(CancellationToken token = default)
    {
        if (_hubConnection is not null)
        {
            await _hubConnection.StopAsync(token);
        }
    }
}