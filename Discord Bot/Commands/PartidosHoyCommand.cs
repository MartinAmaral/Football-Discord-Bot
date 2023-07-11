using Discord;
using Discord.WebSocket;
using Models;

namespace Discord_Bot.Commands
{
    public class PartidosHoyCommand : BaseCommand
    {
        public override string GetCommandName()
        {
            return "partidos_hoy";
        }
        public override SlashCommandBuilder GetCommand()
        {
            var commmand = new SlashCommandBuilder()
                            .WithName("partidos_hoy")
                            .WithDescription("Los partidos de hoy.");
            return commmand;
        }
        public override async Task ExecuteCommand(SocketSlashCommand command)
        {
            await base.ExecuteCommand(command);

            string currentDate = DateTime.Now.ToString("yyyy-MM-dd");
            int leagueId = DateTime.Now.Month <= 6 ? Info.aperturaPrimeraID : Info.clausuraPrimeraID;
            using (HttpResponseMessage response = await FootballApiRequester.apiClient.GetAsync($"/fixtures?date={currentDate}&league={leagueId}&season={DateTime.Now.Year}"))
            {
                response.EnsureSuccessStatusCode();

                var responseBody = await response.Content.ReadAsAsync<FixtureResponse>();

                string responseText = "";
                if (responseBody.Results == 0)
                {
                    responseText = "No hay partidos en el dia de hoy.";
                }
                else
                {
                    foreach (var result in responseBody.Response.OrderBy(x => DateTime.Parse(x.Fixture.Date)))
                    {
                        responseText += $"{result.Teams.Home.Name} vs {result.Teams.Away.Name} a las " +
                            $"{DateTimeOffset.Parse(result.Fixture.Date).ToLocalTime().ToString("HH:mm")}hs en el {result.Fixture.Venue.Name}.\n";
                    }
                }
                await command.RespondAsync(responseText);
            }

        }
    }
}
