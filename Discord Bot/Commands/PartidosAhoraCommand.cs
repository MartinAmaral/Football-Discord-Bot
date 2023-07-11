using Discord;
using Discord.WebSocket;
using Models;

namespace Discord_Bot.Commands
{
    public class PartidosAhoraCommand : BaseCommand
    {
        public override string GetCommandName()
        {
            return "partidos_ahora";
        }
        public override SlashCommandBuilder GetCommand()
        {
            var commmand = new SlashCommandBuilder()
                            .WithName("partidos_ahora")
                            .WithDescription("Los partidos que se estan jugando ahora con los resultados(se actualiza cada 5 min. apox.).");

            return commmand;
        }
        public override async Task ExecuteCommand(SocketSlashCommand command)
        {
            await base.ExecuteCommand(command);

            int leagueId = DateTime.Now.Month <= 6 ? Info.aperturaPrimeraID : Info.clausuraPrimeraID;
            using (HttpResponseMessage response = await FootballApiRequester.apiClient.GetAsync($"/fixtures?live=all&league={leagueId}"))
            {
                response.EnsureSuccessStatusCode();

                var responseBody = await response.Content.ReadAsAsync<FixtureResponse>();

                string responseText = "";

                if (responseBody.Results == 0)
                {
                    responseText = "No se estan jugando partidos en este momento.";
                }
                else
                {
                    foreach (var result in responseBody.Response.OrderBy(x => DateTime.Parse(x.Fixture.Date)))
                    {
                        int homeGoals = result.Goals.HomeTotalGoals.Value;
                        int awayGoals = result.Goals.AwayTotalsGoals.Value;
                        responseText += $"{result.Teams.Home.Name} {homeGoals} - {awayGoals} {result.Teams.Away.Name} a los {result.Fixture.Status.Elapsed} minutos" +
                            $"  en el {result.Fixture.Venue.Name}.\n";
                    }
                }
                await command.RespondAsync(responseText);
            }
        }
    }
}
