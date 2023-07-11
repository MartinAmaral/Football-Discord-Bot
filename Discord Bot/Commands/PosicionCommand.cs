using Discord;
using Discord.WebSocket;
using Models;

namespace Discord_Bot.Commands
{
    public class PosicionCommand : BaseCommand
    {
        public override string GetCommandName()
        {
            return "posicion";
        }
        public override SlashCommandBuilder GetCommand()
        {
            var command = new SlashCommandBuilder()
                .WithName("posicion")
                .WithDescription("Detallada la posicion del equipo, puntos, goles,partidos ganadados, empatados,perdidos.")
                .AddOptions(
                    CommandManager.equipoOption,
                    CommandManager.anoOption,
                    CommandManager.torneoOption
                    );
            return command;
        }
        public override async Task ExecuteCommand(SocketSlashCommand command)
        {
            await base.ExecuteCommand(command);
            var commandsOptions = command.Data.Options.ToList();
            string teamName = commandsOptions[0].Value.ToString();

            if (Info.IsValidTeam(teamName, out var teamID, out var teamCleanName))
            {
                int.TryParse(commandsOptions[1].Value.ToString(), out var ano);
                int.TryParse(commandsOptions[2].Value.ToString(), out var leagueID);

                using (HttpResponseMessage respose = await FootballApiRequester.apiClient.GetAsync($"/standings?league={leagueID}&season={ano}&team={teamID}"))
                {
                    respose.EnsureSuccessStatusCode();
                    var resposeBody = await respose.Content.ReadAsAsync<TeamStandingResponse>();

                    if (resposeBody.Results == 0)
                    {
                        var torneoName = Info.GetTorneoName(leagueID);
                        await command.RespondAsync($"{teamCleanName} no participó en el torneo {torneoName} en el año {ano}.");
                    }
                    else
                    {
                        var team = resposeBody.Response[0].League.Standings[0][0];
                        await command.RespondAsync($"{teamCleanName} se encuentra {team.Rank}° con {team.Points} puntos." +
                            $" Gano {team.All.Win}, perdio {team.All.Lose} y empato {team.All.Draw} partidos.");
                    }
                }
            }
            else await command.RespondAsync($"El nombre del equipo ({teamName}) no es correcto.");
        }


    }
}
