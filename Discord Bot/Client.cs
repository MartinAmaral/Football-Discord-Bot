using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;

namespace Discord_Bot
{
    public class Client
    {
        public DiscordSocketClient client;
        private IConfiguration _configuration;
        public Client(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async void Start()
        {
            client = new DiscordSocketClient();

            client.Log += Log;

            var token = _configuration["DiscordToken"];

            await client.LoginAsync(TokenType.Bot, token);
            await client.StartAsync();

            client.Ready += OnReady;
            client.Disconnected += OnDisconnect;
            client.SlashCommandExecuted += CommandManager.HandleSlashCommands;
        }

        private async Task OnReady()
        {
            await Task.Run(() => CommandManager.Initialize(client));
            await Task.Run(() => FootballApiRequester.InitializeClient(_configuration));
            await DataManager.GetData();
            StartCheckingForLiveMatches();
        }

        private async void StartCheckingForLiveMatches()
        {
            await LiveManager.Initialize();
        }
        private Task OnDisconnect(Exception ex)
        {
            Console.WriteLine($"Disconnect: {ex}");
            return Task.CompletedTask;
        }

        private Task Log(LogMessage message)
        {
            Console.WriteLine(message.ToString() + " (Discord Log).");
            return Task.CompletedTask;
        }
    }
}
