using BYOC.Console;
using Serilog;

var logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.Console()
    .CreateLogger();

Client client = new Client(logger);

await client.StartAsync();

await client.SendAsync("Test", "Message");

await Task.Delay(1_000);