using Discord;
using Discord.WebSocket;
using Models;

namespace Discord_Bot.Commands
{
    public class EstadisticaCommand : BaseCommand
    {
        public override string GetCommandName()
        {
            return "estadisticas";
        }
        public override SlashCommandBuilder GetCommand()
        {
            var commmand = new SlashCommandBuilder()
                            .WithName("estadisticas")
                            .WithDescription("Estadisticas de un equipo para un año y torneo.")
                            .AddOptions(
                                CommandManager.equipoOption,
                                CommandManager.anoOption,
                                CommandManager.torneoOption
                                );
            return commmand;
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

                using (HttpResponseMessage response = await FootballApiRequester.apiClient.GetAsync($"/teams/statistics?league={leagueID}&team={teamID}&season={ano}"))
                {
                    response.EnsureSuccessStatusCode();
                    var resposeBody = await response.Content.ReadAsAsync<TeamStadisticsResponse>();

                    if (resposeBody.Results == 0)
                    {
                        var torneoName = Info.GetTorneoName(leagueID);
                        await command.RespondAsync($"{teamName.UpperFirstLetter()} no participó en el torneo {torneoName} en el año {ano}.");
                    }
                    else
                    {
                        string responseText = "";
                        var body = resposeBody.Response;
                        responseText += $"{teamCleanName} ha jugado {body.Fixtures.Played.Total} partidos. De los cuales gano {body.Fixtures.Wins.Total}" +
                            $", {body.Fixtures.Wins.Home} de local y {body.Fixtures.Wins.Away} de visitante.\nAnoto un total de {body.Goals.For.Total.Total} goles," +
                            $" y recibió {body.Goals.Against.Total.Total}.\nRealizó un total de {body.Penalty.Total} de los cuales anoto {body.Penalty.Scored.Total}" +
                            $" y erró {body.Penalty.Missed.Total}.\n";

                        int cantidadRojas = 0;
                        int cantidadAmarillas = 0;

                        if (body.Cards.yellow.t1.total != null)
                            cantidadAmarillas += (int)body.Cards.yellow.t1.total;
                        if (body.Cards.yellow.t2.total != null)
                            cantidadAmarillas += (int)body.Cards.yellow.t2.total;
                        if (body.Cards.yellow.t3.total != null)
                            cantidadAmarillas += (int)body.Cards.yellow.t3.total;
                        if (body.Cards.yellow.t4.total != null)
                            cantidadAmarillas += (int)body.Cards.yellow.t4.total;
                        if (body.Cards.yellow.t5.total != null)
                            cantidadAmarillas += (int)body.Cards.yellow.t5.total;
                        if (body.Cards.yellow.t6.total != null)
                            cantidadAmarillas += (int)body.Cards.yellow.t6.total;
                        if (body.Cards.yellow.t7.total != null)
                            cantidadAmarillas += (int)body.Cards.yellow.t7.total;
                        if (body.Cards.yellow.t8final.total != null)
                            cantidadAmarillas += (int)body.Cards.yellow.t8final.total;

                        if (body.Cards.red.t1.total != null)
                            cantidadRojas += (int)body.Cards.red.t1.total;
                        if (body.Cards.red.t2.total != null)
                            cantidadRojas += (int)body.Cards.red.t2.total;
                        if (body.Cards.red.t3.total != null)
                            cantidadRojas += (int)body.Cards.red.t3.total;
                        if (body.Cards.red.t4.total != null)
                            cantidadRojas += (int)body.Cards.red.t4.total;
                        if (body.Cards.red.t5.total != null)
                            cantidadRojas += (int)body.Cards.red.t5.total;
                        if (body.Cards.red.t6.total != null)
                            cantidadRojas += (int)body.Cards.red.t6.total;
                        if (body.Cards.red.t7.total != null)
                            cantidadRojas += (int)body.Cards.red.t7.total;
                        if (body.Cards.red.t8final.total != null)
                            cantidadRojas += (int)body.Cards.red.t8final.total;

                        responseText += $"En cuanto a tarjetas recibidas, recibio {cantidadRojas} tajetas rojas y {cantidadAmarillas} tarjetas amarillas.\n";

                        if (cantidadAmarillas == 0 && cantidadRojas == 0)
                            responseText += "(Puede ser que haya un error en la base de datos)";
                        await command.RespondAsync(responseText);
                    }
                }
            }
            else await command.RespondAsync($"El nombre del equipo ({teamName}) no es correcto.");
        }

    }
}
