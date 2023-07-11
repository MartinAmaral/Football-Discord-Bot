using Discord;
using Discord.WebSocket;
using Models;
using System.Numerics;

namespace Discord_Bot.Commands
{
    public class PartidosFechaCommand : BaseCommand
    {
        public override string GetCommandName()
        {
            return "partidos_fecha";
        }
        public override SlashCommandBuilder GetCommand()
        {
            var commmand = new SlashCommandBuilder()
                            .WithName("partidos_fecha")
                            .WithDescription("Todos los partidos de la fecha y resultados.")
                            .AddOptions(
                                CommandManager.fechaOption,
                                CommandManager.torneoOption,
                                CommandManager.anoOption
                                );
            return commmand;
        }
        public override async Task ExecuteCommand(SocketSlashCommand command)
        {
            await base.ExecuteCommand(command);
            var options = command.Data.Options.ToList();

            int.TryParse(options[0].Value.ToString(), out var fecha);
            int.TryParse(options[1].Value.ToString(), out var torneoID);
            int.TryParse(options[2].Value.ToString(), out var ano);

            if (fecha < 1 || fecha > 15)
            {
                await command.RespondAsync($"Fecha invalida.");
            }
            else
            {
                FixtureResponse fechaData;
                if (DataManager.storedFixtures.TryGetValue(new Vector2(torneoID, ano), out var storedResponse))
                {
                    fechaData = storedResponse;
                }
                else
                {
                    using (HttpResponseMessage response = await FootballApiRequester.apiClient.GetAsync($"/fixtures?league={torneoID}&season={ano}&round=Apertura -{fecha}"))
                    {
                        response.EnsureSuccessStatusCode();

                        fechaData = await response.Content.ReadAsAsync<FixtureResponse>();

                    }
                }

                string respond = "";

                var filteredData = fechaData.Response.Where(x => x.League.round.EndsWith($"- {fecha}")).OrderBy(x => DateTimeOffset.Parse(x.Fixture.Date)).ToList();
                for (int i = 0; i < 8; i++)
                {
                    var fixture = filteredData[i];
                    respond += $"{fixture.Teams.Home.Name} {fixture.Goals.HomeTotalGoals} - {fixture.Goals.AwayTotalsGoals} {fixture.Teams.Away.Name}" +
                        $" a las {DateTimeOffset.Parse(fixture.Fixture.Date).ToString("HH:mm")} en el {fixture.Fixture.Venue.Name}.\n";
                }
                await command.RespondAsync(respond);
            }
        }

    }
}
