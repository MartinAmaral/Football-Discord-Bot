using Discord;
using Discord.WebSocket;
using Models;

namespace Discord_Bot.Commands
{
    public class ComoSalieronCommand : BaseCommand
    {
        public override string GetCommandName()
        {
            return "como_salieron";
        }
        public override SlashCommandBuilder GetCommand()
        {
            var commmand = new SlashCommandBuilder()
                            .WithName("como_salieron")
                            .WithDescription("Resultado de un partido entres dos equipos en un torneo y año.")
                            .AddOption(CommandManager.equipoOption)
                            .AddOption(CommandManager.equipoOption2)
                            .AddOption(CommandManager.torneoOption)
                            .AddOption(CommandManager.anoOption);

            return commmand;
        }
        public override async Task ExecuteCommand(SocketSlashCommand command)
        {
            await base.ExecuteCommand(command);

            var commandsOptions = command.Data.Options.ToList();
            string teamName1 = commandsOptions[0].Value.ToString();
            string teamName2 = commandsOptions[1].Value.ToString();
            int leagueID = int.Parse(commandsOptions[2].Value.ToString());
            int ano = int.Parse(commandsOptions[3].Value.ToString());


            if (Info.IsValidTeam(teamName1, out var team1ID, out var teamCleanName1) && Info.IsValidTeam(teamName2, out var team2ID, out var teamCleanName2))
            {

                using (HttpResponseMessage response = await FootballApiRequester.apiClient.GetAsync($"/fixtures/headtohead?league={leagueID}&season={ano}&h2h={team1ID}-{team2ID}"))
                {
                    response.EnsureSuccessStatusCode();

                    var responseBody = await response.Content.ReadAsAsync<FixtureResponse>();

                    string responseText = "";
                    if (responseBody.Results == 0)
                    {
                        responseText = "No se encontraron partidos entre estos equipos.";
                    }
                    else
                    {
                        foreach (var result in responseBody.Response.OrderBy(x => DateTime.Parse(x.Fixture.Date)))
                        {
                            if (result.Goals.AwayTotalsGoals == null)
                            {
                                responseText += $"Ese partido todavía no se ha jugado. Se jugara el {DateTimeOffset.Parse(result.Fixture.Date).ToString("dd/MM")} en el {result.Fixture.Venue.Name}" +
                                    $" a las {DateTimeOffset.Parse(result.Fixture.Date).ToString("HH:mm")}.\n";
                            }
                            else
                            {
                                string rounds = result.League.round.Substring(result.League.round.Length - 2);
                                string round = rounds.Replace(" ", "");
                                responseText += $"{result.Teams.Home.Name} {result.Goals.HomeTotalGoals} - {result.Goals.AwayTotalsGoals} {result.Teams.Away.Name} " +
                                    $"en el {result.Fixture.Venue.Name} por la fecha {round} en el en el dia " +
                                    $"{DateTimeOffset.Parse(result.Fixture.Date).ToString("dd-MM")} .\n";
                            }
                        }
                    }
                    await command.RespondAsync(responseText);
                }
            }
            else
            {
                await command.RespondAsync("No son equipos validos");
            }

        }
    }
}
