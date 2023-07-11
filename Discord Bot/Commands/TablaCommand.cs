using Discord;
using Discord.WebSocket;
using Models;

namespace Discord_Bot.Commands
{
    public class TablaCommand : BaseCommand
    {
        public override string GetCommandName()
        {
            return "tabla";
        }
        public override SlashCommandBuilder GetCommand()
        {
            var command = new SlashCommandBuilder()
                .WithName("tabla")
                .WithDescription("Devuelve la tabla con el resultado de los equipos.")
                .AddOptions(
                    CommandManager.torneoOption,
                    CommandManager.anoOption
                    );
            return command;
        }
        public override async Task ExecuteCommand(SocketSlashCommand command)
        {
            await base.ExecuteCommand(command);
            var commandsOptions = command.Data.Options.ToList();

            int.TryParse(commandsOptions[0].Value.ToString(), out var leagueID);
            int.TryParse(commandsOptions[1].Value.ToString(), out var ano);

            using (HttpResponseMessage respose = await FootballApiRequester.apiClient.GetAsync($"/standings?league={leagueID}&season={ano}"))
            {
                respose.EnsureSuccessStatusCode();
                var responseBody = await respose.Content.ReadAsAsync<LeagueStandingsResponse>();

                if (responseBody.Results != 0)
                {
                    var body = responseBody.response[0].League;

                    string response = "";

                    if (body.Standings[0][1].Points != 0)
                    {
                        for (int i = 0; i < body.Standings[0].Length; i++)
                        {

                            Info.IsValidTeam(body.Standings[0][i].Team.Name, out int id, out string name);
                            response += $"{body.Standings[0][i].Rank}° {name}, puntos {body.Standings[0][i].Points}," +
                                $" partidos ganados {body.Standings[0][i].Matches.Win}, empatados {body.Standings[0][1].Matches.Draw}," +
                                $" perdidos {body.Standings[0][i].Matches.Lose}." +
                                $" Dif. de goles {body.Standings[0][i].GoalsDiff}, a favor {body.Standings[0][i].Matches.Goals.For}, en contra {body.Standings[0][i].Matches.Goals.Against}." +
                                $"\n";
                        }
                    }
                    else response = $"Hubo un error en la base de datos. No se encontraron los datos.";
                    Console.WriteLine(response.Length); // this command is almost reaching the character limits of a reponse in discord.
                    await command.RespondAsync(response);
                }
                else
                {
                    await command.RespondAsync("No se puede encontrar ese torneo con ese año.");
                }
            }
        }

    }
}
