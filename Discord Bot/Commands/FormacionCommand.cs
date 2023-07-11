namespace Discord_Bot.Commands
{
    /* The api doesnt work properly
    public class FormacionCommand : BaseCommand
    {
        public override string GetCommandName()
        {
            return "formacion";
        }
        public override SlashCommandBuilder GetCommand()
        {
            var commmand = new SlashCommandBuilder()
                            .WithName("formacion")
                            .WithDescription("Formacion incial de un equipo a tal fecha.")
                            .AddOptions(
                                CommandManager.equipoOption,
                                CommandManager.fechaOption,
                                CommandManager.torneoOption,
                                CommandManager.anoOption
                                ); ;
            return commmand;
        }
        public override async Task ExecuteCommand(SocketSlashCommand command)
        {
            Console.WriteLine($"Executing {GetCommandName()} command.");
            var options = command.Data.Options.ToList();

            string respondText = "";

            if (Info.IsValidTeam(options[0].Value.ToString(), out var teamID, out var teamCleanName))
            {
                int.TryParse(options[1].Value.ToString(), out var fecha);
                int.TryParse(options[2].Value.ToString(), out var torneoID);
                int.TryParse(options[3].Value.ToString(), out var ano);

                if (fecha < 1 || fecha > 15)
                {
                    respondText = "No es una fecha valida";
                }
                else
                {
                    FixtureResponse fixtureData;
                    if (DataManager.storedFixtures.TryGetValue(new Vector2(torneoID, ano), out var storedResponse))
                    {
                        fixtureData = storedResponse;
                        var j = storedResponse.Response.Where((x) => x.League.round.EndsWith($" {fecha}")).ToArray();
                        fixtureData.Response = j;
                    }
                    else
                    {
                        using (HttpResponseMessage response = await FootballApiRequester.apiClient.GetAsync($"/fixtures?league={torneoID}&season={ano}&round=Apertura -{fecha}"))
                        {
                            response.EnsureSuccessStatusCode();

                            fixtureData = await response.Content.ReadAsAsync<FixtureResponse>();

                        }
                    }

                    if (fixtureData.Results != 0)
                    {

                        fixtureData.Response = fixtureData.Response.Where((x) => x.Teams.Away.Id == teamID || x.Teams.Home.Id == teamID).ToArray();

                        using (HttpResponseMessage response = await FootballApiRequester.apiClient.GetAsync($"/fixtures/lineups?fixture={fixtureData.Response[0].Fixture.Id}"))
                        {
                            response.EnsureSuccessStatusCode();


                            string jsonResponse = await response.Content.ReadAsStringAsync();
                            string indentedJsonResponse = JsonConvert.SerializeObject(JsonConvert.DeserializeObject(jsonResponse), Formatting.Indented);
                            Console.WriteLine(indentedJsonResponse);



                            var responseBody = await response.Content.ReadAsAsync<FormacionResponse>();

                            if (responseBody.Results != 0)
                            {
                                var team = responseBody.Response.Where((x) => x.Team.Id == teamID).First();

                                respondText += $"{teamCleanName} jugó con formacion {team.formation}.\n";
                                foreach (var player in team.startXI)
                                {
                                    respondText += $"{player.Name} con la {player.Number}.\n";
                                }
                            }
                            else respondText = "No se encontro el partido";

                        }
                    }
                    else
                    {
                        respondText = "No se encontraron datos";
                    }
                }
            }
            else
            {
                respondText = "No es un equipo valido";
            }
            await command.RespondAsync(respondText);
        }
    }*/
}
