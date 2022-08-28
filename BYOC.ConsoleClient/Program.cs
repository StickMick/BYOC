using BYOC.ConsoleClient;
using BYOC.ConsoleClient.SignalrClient;
using Serilog;

const string apiKey = "199970c4-3b57-4395-b8fe-35419fd8e05b";

var logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.Console()
    .CreateLogger();

Client client = new Client(logger);

CancellationTokenSource cts = new CancellationTokenSource();
CancellationToken token = cts.Token;

await client.StartAsync(apiKey ,token);

ConsoleController controller = new ConsoleController(client, logger, cts);

while (client.IsConnected)
{
    // await controller.DrawWorld(token);
    // await controller.HandleInput(token);
}