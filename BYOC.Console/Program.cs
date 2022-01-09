using BYOC.Console;

Client client = new Client();

await client.StartAsync();

Console.WriteLine(client.IsConnected);

await client.SendAsync("Test", "Message");

await Task.Delay(1_000);