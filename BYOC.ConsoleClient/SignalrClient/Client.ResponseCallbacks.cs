using BYOC.ConsoleClient.Helpers;
using BYOC.Shared.DTOs;
using Microsoft.AspNetCore.SignalR.Client;

namespace BYOC.ConsoleClient.SignalrClient;

public partial class Client
{
    private void BindResponseCallbacks()
    {
        if (_hubConnection == null)
            return;
        
        _hubConnection.On<WorldDTO>("GetWorldAsync", MapVisualizer.DrawToConsole);

        _hubConnection.On<UnitMovedEventDto>("UnitMoved", (e) =>
        {
            Console.WriteLine("Unit {0} moved to x:{1} , y:{2}", e.Id, e.Position.X, e.Position.Y);
        });
    }
}