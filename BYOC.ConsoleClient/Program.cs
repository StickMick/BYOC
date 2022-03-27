using BYOC.ConsoleClient;
using Serilog;

var logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.Console()
    .CreateLogger();

Client client = new Client(logger);

CancellationTokenSource cts = new CancellationTokenSource();
CancellationToken token = cts.Token;

await client.StartAsync(token);

ConsoleController controller = new ConsoleController(client, logger, cts);

while (client.IsConnected)
{
    await controller.HandleInput(token);
}