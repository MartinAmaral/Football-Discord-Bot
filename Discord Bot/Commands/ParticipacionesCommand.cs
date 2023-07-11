using Discord;
using Discord.WebSocket;
using Models;

namespace Discord_Bot.Commands
{
    public class ParticipacionesCommand : BaseCommand
    {
        public override string GetCommandName()
        {
            return "participaciones";
        }
        public override SlashCommandBuilder GetCommand()
        {
            var command = new SlashCommandBuilder()
                .WithName("participaciones")
                .WithDescription("Los años en que el equipo participo del torneo elegido.")
                .AddOptions(
                    CommandManager.equipoOption,
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
                using (HttpResponseMessage respose = await FootballApiRequester.apiClient.GetAsync($"/leagues?team={teamID}"))
                {
                    respose.EnsureSuccessStatusCode();

                    var resposeBody = await respose.Content.ReadAsAsync<LeagueTeamResponse>();

                    var body = resposeBody.Response;

                    int leagueID = int.Parse(commandsOptions[1].Value.ToString());
                    List<int> anos = new List<int>();

                    foreach (var response in body)
                    {
                        if (response.League.Id == Info.aperturaPrimeraID)
                        {
                            foreach (var item in response.Seasons)
                            {
                                anos.Add(item.year);
                            }
                        }
                    }

                    string leagueName = Info.GetTorneoName(leagueID);
                    string respond = "";
                    if (anos.Count == 0)
                    {
                        respond += $"{teamCleanName} no ha participado del {leagueName} desde el año 2012 en adelante.\n";
                    }
                    else
                    {
                        string plural = anos.Count > 1 ? "los años" : "el año";
                        respond += $"{teamCleanName} ha participado del {leagueName} en {plural}: ";
                        foreach (var item in anos)
                        {
                            respond += $"{item}, ";
                        }
                        respond = respond.Substring(0, respond.Length - 2);
                        respond += ".\n";
                    }

                    await command.RespondAsync(respond);
                }
            }
            else await command.RespondAsync($"El nombre del equipo ({teamName}) no es correcto.");
        }

    }
}
