using Discord;
using Discord.WebSocket;
using Models;

namespace Discord_Bot.Commands
{
    public class GoleadoresCommand : BaseCommand
    {
        public override string GetCommandName()
        {
            return "goleadores_torneo";
        }
        public override SlashCommandBuilder GetCommand()
        {
            var commmand = new SlashCommandBuilder()
                            .WithName("goleadores_torneo")
                            .WithDescription("Los 10 maximo goleadores del torneo para ese año.")
                            .AddOptions(
                                CommandManager.torneoOption,
                                CommandManager.anoOption
                                );
            return commmand;
        }

        public override async Task ExecuteCommand(SocketSlashCommand command)
        {
            await base.ExecuteCommand(command);
            var options = command.Data.Options.ToList();

            int.TryParse(options[0].Value.ToString(), out var leagueID);
            int.TryParse(options[1].Value.ToString(), out var ano);

            using (HttpResponseMessage response = await FootballApiRequester.apiClient.GetAsync($"/players/topscorers?league={leagueID}&season={ano}"))
            {
                response.EnsureSuccessStatusCode();

                var responseBody = await response.Content.ReadAsAsync<TopScorersResponse>();
                string respond = "";

                if (responseBody.Results != 0)
                {
                    for (int i = 0; i < Math.Min(10, responseBody.Results); i++)
                    {
                        var body = responseBody.Response[i];
                        Info.IsValidTeam(body.Statistics[0].Team.Name, out var teamID, out var teamCleanName);
                        respond += $"{i + 1}° {body.Player.Name} con {body.Statistics[0].Goals.Total} goles, jugando para {teamCleanName}.\n";
                    }
                }
                else
                {
                    respond += "No se encontraron los goleadores para ese año.";
                }
                await command.RespondAsync(respond);
            }
        }
    }
}
