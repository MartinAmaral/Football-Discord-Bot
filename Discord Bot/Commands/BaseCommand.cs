using Discord;
using Discord.WebSocket;

namespace Discord_Bot.Commands
{
    public abstract class BaseCommand
    {
        public abstract string GetCommandName();
        public abstract SlashCommandBuilder GetCommand();
        public virtual async Task ExecuteCommand(SocketSlashCommand command)
        {
            Console.WriteLine($"Executing {GetCommandName()} command.");
        }

    }
}
