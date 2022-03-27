using Microsoft.AspNetCore.SignalR.Client;

namespace BYOC.ConsoleClient;

public partial class Client
{
    public async Task GetWorldAsync(CancellationToken token = default)
    {
        if (_hubConnection is not null && IsConnected)
        {
            await _hubConnection.InvokeAsync<string>("GetWorldAsync", token);
        }
    }
}