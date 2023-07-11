using Discord_Bot;
using Microsoft.Extensions.Configuration;

class Program
{
    private Client _client;
    static Task Main(string[] args)
    {
        return new Program().MainAsync();
    }

    public async Task MainAsync()
    {
        var builder = new ConfigurationBuilder().AddUserSecrets<Client>();
        _client = new Client(builder.Build());
        _client.Start();

        await Task.Delay(-1);
    }

}


