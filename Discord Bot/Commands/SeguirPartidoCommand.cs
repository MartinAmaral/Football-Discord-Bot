using Discord;
using Discord.WebSocket;
using Models;

namespace Discord_Bot.Commands
{
    public class SeguirPartidoCommand : BaseCommand
    {
        public override string GetCommandName()
        {
            return "seguir_partido";
        }
        public override SlashCommandBuilder GetCommand()
        {
            var commmand = new SlashCommandBuilder()
                            .WithName("seguir_partido")
                            .WithDescription("Seguir partido en vivo y recibir actualizaciones (cada 2 minutos).")
                            .AddOptions(
                                CommandManager.equipoOption
                                );
            return commmand;
        }
        public override async Task ExecuteCommand(SocketSlashCommand command)
        {
            await base.ExecuteCommand(command);
            var commandsOptions = command.Data.Options.ToList();
            string teamName = commandsOptions[0].Value.ToString();

            string responseText = "";

            if (Info.IsValidTeam(teamName, out var teamID, out var teamCleanName))
            {
                string currentDate = DateTime.Now.ToString("yyyy-MM-dd");
                int leagueId = DateTime.Now.Month <= 6 ? Info.aperturaPrimeraID : Info.clausuraPrimeraID;

                using (HttpResponseMessage response = await FootballApiRequester.apiClient.GetAsync($"/fixtures?date={currentDate}&league={leagueId}&season={DateTime.Now.Year}"))
                {
                    response.EnsureSuccessStatusCode();

                    var responseBody = await response.Content.ReadAsAsync<FixtureResponse>();

                    var match = responseBody.Response.Where((x) => x.Teams.Home.Id == teamID || x.Teams.Away.Id == teamID).FirstOrDefault();

                    if (match != null)
                    {
                        await LiveManager.FollowMatch(match, command);
                        responseText = "El partido se esta siguiendo.";
                    }
                    else
                    {
                        responseText += "No se encontro partido o el equipo no juega hoy";
                    }
                }
            }
            else
            {
                responseText = "Equipo invalido.";
            }
            await command.RespondAsync(responseText);
        }
    }
}
