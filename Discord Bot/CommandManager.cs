using Discord;
using Discord.Net;
using Discord.WebSocket;
using Discord_Bot.Commands;
using Newtonsoft.Json;
using System.Reflection;

namespace Discord_Bot
{
#pragma warning disable CS8618

    public static class CommandManager
    {
        private static DiscordSocketClient _client;

        public static SlashCommandOptionBuilder torneoOption;
        public static SlashCommandOptionBuilder anoOption;
        public static SlashCommandOptionBuilder equipoOption;
        public static SlashCommandOptionBuilder equipoOption2;
        public static SlashCommandOptionBuilder fechaOption;

        public static Dictionary<string, BaseCommand> _commands = new();

        public static async void Initialize(DiscordSocketClient client)
        {
            _client = client;
            await PopulateCommands();
        }

        public static async Task PopulateCommands()
        {
            torneoOption = new SlashCommandOptionBuilder()
                    .WithName("torneo")
                    .WithRequired(true)
                    .WithDescription("Clausura o apertura")
                    .AddChoice("Apertura", Info.aperturaPrimeraID)
                    .AddChoice("Clasura", Info.clausuraPrimeraID)
                    .WithType(ApplicationCommandOptionType.Integer);

            anoOption = new SlashCommandOptionBuilder()
                    .WithName("año")
                    .WithType(ApplicationCommandOptionType.Integer)
                    .WithDescription("Año")
                    .AddChoice("2012", 2012)
                    .AddChoice("2013", 2013)
                    .AddChoice("2014", 2014)
                    .AddChoice("2015", 2015)
                    .AddChoice("2016", 2016)
                    .AddChoice("2017", 2017)
                    .AddChoice("2018", 2018)
                    .AddChoice("2019", 2019)
                    .AddChoice("2020", 2020)
                    .AddChoice("2021", 2021)
                    .AddChoice("2022", 2022)
                    .AddChoice("2023", 2023)
                    .WithRequired(true);
            equipoOption = new SlashCommandOptionBuilder()
                    .WithName("equipo")
                    .WithType(ApplicationCommandOptionType.String)
                    .WithDescription("Equipo a mostrar")
                    .WithRequired(true);
            equipoOption2 = new SlashCommandOptionBuilder()
                    .WithName("equipo2")
                    .WithType(ApplicationCommandOptionType.String)
                    .WithDescription("Segundo equipo a mostrar")
                    .WithRequired(true);
            fechaOption = new SlashCommandOptionBuilder()
                .WithName("fecha")
                .WithType(ApplicationCommandOptionType.Integer)
                .WithDescription("Fecha a revisar")
                .WithRequired(true);

            var implementingTypes = Assembly.GetExecutingAssembly().GetTypes()
            .Where(t => t.IsSubclassOf(typeof(BaseCommand)) && t.IsClass);

            List<SlashCommandBuilder> slashCommands = new List<SlashCommandBuilder>();
            foreach (var type in implementingTypes)
            {
                var x = (BaseCommand)Activator.CreateInstance(type);
                slashCommands.Add(x.GetCommand());
                _commands.Add(x.GetCommandName(), x);
            }

            Console.WriteLine("Numero de comandos: " + slashCommands.Count);

            try
            {
                foreach (SocketGuild guild in _client.Guilds)
                {
                    var storedCommands = await guild.GetApplicationCommandsAsync();

                    foreach (var command in slashCommands)
                    {
                        if (!storedCommands.Any(stored => stored.Name == command.Name))
                        {
                            await guild.CreateApplicationCommandAsync(command.Build());
                        }
                    }
                }
            }
            catch (ApplicationCommandException exception)
            {

                var jason = JsonConvert.SerializeObject(exception.Errors, Formatting.Indented);

                Console.WriteLine(jason);
            }
        }


        public static async Task HandleSlashCommands(SocketSlashCommand command)
        {
            if (_commands.TryGetValue(command.Data.Name, out var result))
            {
                await result.ExecuteCommand(command);
            }
            else
            {
                await command.RespondAsync("Ese comando no exite.");
            }
        }
    }
}
