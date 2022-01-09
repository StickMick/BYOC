using Microsoft.AspNetCore.SignalR.Client;

namespace BYOC.Console;

public class Client
{
    private HubConnection? hubConnection;
    private List<string> messages = new List<string>();
    private string? userInput;
    private string? messageInput;

    public async Task StartAsync()
    {
        var handler = new HttpClientHandler();
        handler.ClientCertificateOptions = ClientCertificateOption.Manual;
        handler.ServerCertificateCustomValidationCallback = 
            (httpRequestMessage, cert, cetChain, policyErrors) =>
            {
                return true;
            };

        var client = new HttpClient(handler);

        
        hubConnection = new HubConnectionBuilder()
            .WithUrl("https://localhost:7111/demo", (opts) =>
            {
                opts.HttpMessageHandlerFactory = (message) =>
                {
                    if (message is HttpClientHandler clientHandler)
                        // always verify the SSL certificate
                        clientHandler.ServerCertificateCustomValidationCallback +=
                            (sender, certificate, chain, sslPolicyErrors) => { return true; };
                    return message;
                };
            })
            .Build();

        hubConnection.On<string, string>("ReceiveMessage", (user, message) =>
        {
            var encodedMsg = $"{user}: {message}";
            System.Console.WriteLine(encodedMsg);
        });

        await hubConnection.StartAsync();
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