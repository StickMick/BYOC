using BYOC.ConsoleClient.Helpers;
using BYOC.ConsoleClient.SignalrClient;
using BYOC.Data.Objects;
using BYOC.Shared.DTOs;
using Serilog;

namespace BYOC.ConsoleClient;

public class ConsoleController
{
    private readonly Client _client;
    private readonly ILogger _logger;
    private readonly CancellationTokenSource _cancellationTokenSource;

    public ConsoleController(Client client,
        ILogger logger,
        CancellationTokenSource cancellationTokenSource)
    {
        _client = client;
        _logger = logger;
        _cancellationTokenSource = cancellationTokenSource;
    }
    
    public async Task HandleInput(CancellationToken token = default)
    {
        Console.WriteLine();
        Console.Write("Command: ");
        var command = Console.ReadLine()?.ToLower();

        switch (command)
        {
            case "get world":
                await _client.GetWorldAsync(token);
                break;
            case "exit":
                await _client.Disconnect(token);
                _cancellationTokenSource.Cancel();
                _logger.Information("Exiting...");
                break;
            default:
                Console.WriteLine("Unknown Command {0}", command);
                break;
        }

        await Task.Delay(1000, token);
    }

    public async Task DrawWorld(CancellationToken token)
    {
        await _client.GetWorldAsync(token);
        await Task.Delay(1000, token);
    }
}