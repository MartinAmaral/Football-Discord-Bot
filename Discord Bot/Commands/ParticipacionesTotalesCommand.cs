using Discord;
using Discord.WebSocket;
using Models;

namespace Discord_Bot.Commands
{
    public class ParticipacionesTotalesCommand : BaseCommand
    {
        public override string GetCommandName()
        {
            return "participaciones_totales";
        }
        public override SlashCommandBuilder GetCommand()
        {
            var command = new SlashCommandBuilder()
                .WithName("participaciones_totales")
                .WithDescription("Los años en que el equipo participo del Torneo Apertura y Clausura.")
                .AddOptions(
                    CommandManager.equipoOption
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

                    List<int> anosApertura = new List<int>();
                    List<int> anosClausura = new List<int>();

                    foreach (var response in body)
                    {
                        if (response.League.Id == Info.aperturaPrimeraID)
                        {
                            foreach (var item in response.Seasons)
                            {
                                anosApertura.Add(item.year);
                            }
                        }
                        else if (response.League.Id == Info.clausuraPrimeraID)
                        {
                            foreach (var item in response.Seasons)
                            {
                                anosClausura.Add(item.year);
                            }
                        }
                    }

                    List<int> list = anosApertura;
                    string league = "Torneo Apertura";
                    string respond = "";
                    int counter = 0;
                    while (counter < 2)
                    {
                        if (list.Count == 0)
                        {
                            respond += $"{teamCleanName} no ha participado del {league} desde el año 2012 en adelante.\n";
                        }
                        else
                        {
                            string plural = anosApertura.Count > 1 ? "los años" : "el año";
                            respond += $"{teamCleanName} ha participado del {league} en {plural}: ";
                            foreach (var item in anosApertura)
                            {
                                respond += $"{item}, ";
                            }
                            respond = respond.Substring(0, respond.Length - 2);
                            respond += ".\n";
                        }
                        league = "Torneo Clausura";
                        list = anosClausura;
                        counter++;
                    }
                    await command.RespondAsync(respond);
                }
            }
            else await command.RespondAsync($"El nombre del equipo ({teamName}) no es correcto.");
        }
    }
}
