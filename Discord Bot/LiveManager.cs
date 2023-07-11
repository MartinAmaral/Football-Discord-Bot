using Discord.WebSocket;
using Models;
using Models.Events;
using Models.Fixture;

namespace Discord_Bot
{
    public static class LiveManager
    {
        private static List<FollowedMatch> followingMatches = new();

        public static async Task Initialize()
        {
            while (true)
            {
                Console.WriteLine($"{DateTime.Now.ToString("HH:mm:ss")} Checking matches");
                await CheckMatches();
                await Task.Delay(TimeSpan.FromMinutes(2.1));
            }
        }
        public static async Task FollowMatch(PartidosFechaModel fixtureToAdd, SocketSlashCommand command)
        {
            bool matchAlreadyExist = false;

            foreach (var match in followingMatches)
            {
                if (match.FixtureID == fixtureToAdd.Fixture.Id)
                {
                    matchAlreadyExist = true;
                    bool channelAlreadyExist = false;
                    foreach (var x in match.channelsUser.Keys)
                    {
                        if (x.GuildId == command.GuildId)
                        {
                            channelAlreadyExist = true;

                            match.channelsUser.TryGetValue(x, out var users);
                            users.Add(command.User.Mention);
                            Console.WriteLine($"Added a new user to a followed match, use:{command.User.Mention} ");
                            break;
                        }
                    }
                    if (!channelAlreadyExist)
                    {
                        Console.WriteLine($"Added a new guild {command.GuildId}, user {command.User.Mention}" +
                            $" to a existing match {fixtureToAdd.Fixture.Id} teams {fixtureToAdd.Teams.Home.Name} and" +
                            $" {fixtureToAdd.Teams.Away.Name}.");
                        match.channelsUser.Add(command, new HashSet<string> { command.User.Mention });
                    }
                }
            }

            if (!matchAlreadyExist)
            {
                Console.WriteLine($"Adding a new match to follow {command.Channel.Name}, user {command.User.Mention}" +
                            $" FixtureId: {fixtureToAdd.Fixture.Id} teams {fixtureToAdd.Teams.Home.Name} and" +
                            $" {fixtureToAdd.Teams.Away.Name}.");
                followingMatches.Add(new FollowedMatch(fixtureToAdd.Fixture.Id, fixtureToAdd.Fixture.Date, command));
            }
        }

        private static async Task CheckMatches()
        {
            Console.WriteLine($"Tracking Matches amount {followingMatches.Count}");
            List<FollowedMatch> matchesToRemove = new();
            string messageToSend = "";
            foreach (var match in followingMatches)
            {
                var matchTime = DateTimeOffset.Parse(match.StartingDate).ToLocalTime();
                var now = DateTimeOffset.Now.ToLocalTime();

                if ((now - matchTime).TotalMinutes >= 0)
                {
                    if ((now - matchTime).TotalMinutes < 90 + 20)
                    {
                        Console.WriteLine($"Match is under 110 minutes since begging.");
                        messageToSend = await CheckEvents(match, messageToSend);
                        await SendMessageToChannel(match, messageToSend);
                    }
                    else // if its been more than 110 minutes since started, check if the match has finished
                    {
                        using (HttpResponseMessage response = await FootballApiRequester.apiClient.GetAsync($"/fixtures?id={match.FixtureID}"))
                        {
                            response.EnsureSuccessStatusCode();
                            var x = await response.Content.ReadAsAsync<FixtureResponse>();
                            var y = x.Response[0];

                            if (y.Fixture.Status.MatchStatus == "Match Finished")
                            {
                                matchesToRemove.Add(match);
                                await SendMessageToChannel(match, $"El partido ha finalizado. {y.Teams.Home.Name}" +
                                    $" {y.Goals.HomeTotalGoals} - {y.Goals.AwayTotalsGoals} {y.Teams.Away.Name}");
                            }
                            else
                            {
                                messageToSend = await CheckEvents(match, messageToSend);
                                await SendMessageToChannel(match, messageToSend);
                            }
                        }
                    }
                }
                else Console.WriteLine("Match hasn't started yet");
            }

            foreach (var match in matchesToRemove)
            {
                Console.WriteLine($"Removing match {match.FixtureID}");
                followingMatches.Remove(match);
            }
        }

        public static async Task SendMessageToChannel(FollowedMatch match, string message)
        {
            foreach (var item in match.channelsUser.Keys)
            {
                HashSet<string> users = match.channelsUser[item];
                string usersToTag = "";
                foreach (var user in users)
                {
                    usersToTag += $"{user} ";
                }
                if (message != "")
                {
                    await item.Channel.SendMessageAsync($"{usersToTag} {message}");
                }
            }
        }

        private static async Task<string> CheckEvents(FollowedMatch match, string messageToSend)
        {
            using (HttpResponseMessage response = await FootballApiRequester.apiClient.GetAsync($"/fixtures/events?fixture={match.FixtureID}"))
            {
                response.EnsureSuccessStatusCode();
                var allEvents = await response.Content.ReadAsAsync<EventsResponse>();
                Console.WriteLine("Amount of results:" + allEvents.Results);
                Console.WriteLine("Amount of stores events" + match.storedEvents.Count);

                if (allEvents.Results != match.storedEvents.Count)
                {
                    for (int i = match.storedEvents.Count; i < allEvents.Results; i++)
                    {
                        messageToSend += GetMessage(allEvents.Response[i]);
                        match.storedEvents.Add(allEvents.Response[i]);
                    }
                }
            }

            return messageToSend;
        }

        private static string GetMessage(EventModel matchEvent)
        {
            switch (matchEvent.Type)
            {
                case "Goal":
                    switch (matchEvent.Detail)
                    {
                        case "Normal Goal":
                            string normalGoalText = $"{matchEvent.Player.Name} metio un gol para {matchEvent.Team.Name} a los {matchEvent.Time.Elapsed}";
                            if (matchEvent.Time.extra == null)
                            {
                                normalGoalText += ".\n";
                            }
                            else normalGoalText += $" + {matchEvent.Time.extra} de tiempo Adicional.\n";
                            return normalGoalText;
                        case "Own Goal":
                            string ownGoalText = $"{matchEvent.Player.Name} metio un gol en contra jugando para {matchEvent.Team.Name} a los {matchEvent.Time.Elapsed}";
                            if (matchEvent.Time.extra == null)
                            {
                                ownGoalText += ".\n";
                            }
                            else ownGoalText += $" + {matchEvent.Time.extra} de tiempo Adicional.\n";
                            return ownGoalText;
                        case "Penalty":
                            string penaltyText = $"{matchEvent.Player.Name} metio un gol de penal a favor de {matchEvent.Team.Name} a los {matchEvent.Time.Elapsed}";
                            if (matchEvent.Time.extra == null)
                            {
                                penaltyText += ".\n";
                            }
                            else penaltyText += $" + {matchEvent.Time.extra} de tiempo Adicional.\n";
                            return penaltyText;
                        case "Missed Penalty":
                            string missedPenaltyText = $"{matchEvent.Player.Name} erro un gol de penal a favor de {matchEvent.Team.Name} a los {matchEvent.Time.Elapsed}";
                            if (matchEvent.Time.extra == null)
                            {
                                missedPenaltyText += ".\n";
                            }
                            else missedPenaltyText += $" + {matchEvent.Time.extra} de tiempo Adicional.\n";
                            return missedPenaltyText;
                    }
                    break;
                case "Card":
                    if (matchEvent.Detail == "Yellow Card")
                    {
                        string yellowCardText = $"{matchEvent.Player.Name} recibio una tarjeta amarrilla jugando para {matchEvent.Team.Name} a los {matchEvent.Time.Elapsed}";
                        if (matchEvent.Time.extra == null)
                        {
                            yellowCardText += ".\n";
                        }
                        else yellowCardText += $" + {matchEvent.Time.extra} de tiempo Adicional.\n";
                        return yellowCardText;
                    }
                    else if (matchEvent.Detail == "Red Card")
                    {
                        string redCardText = $"{matchEvent.Player.Name} recibio una tarjeta roja jugando para {matchEvent.Team.Name} a los {matchEvent.Time.Elapsed}";
                        if (matchEvent.Time.extra == null)
                        {
                            redCardText += ".\n";
                        }
                        else redCardText += $" + {matchEvent.Time.extra} de tiempo Adicional.\n";
                        return redCardText;
                    }
                    break;
                case "subst":
                    string SubstText = $"{matchEvent.Player.Name} de {matchEvent.Team.Name} acaba de entrar por {matchEvent.Assist.Name} a los {matchEvent.Time.Elapsed}";
                    if (matchEvent.Time.extra == null)
                    {
                        SubstText += ".\n";
                    }
                    else SubstText += $" + {matchEvent.Time.extra} de tiempo Adicional.\n";
                    return SubstText;
                case "Var":
                    if (matchEvent.Detail == "Goal cancelled")
                    {
                        string goalCancelledText = $"Le acaban de cancelar un goal a {matchEvent.Team.Name} a los {matchEvent.Time.Elapsed}";
                        if (matchEvent.Time.extra == null)
                        {
                            goalCancelledText += ".\n";
                        }
                        else goalCancelledText += $" + {matchEvent.Time.extra} de tiempo Adicional.\n";
                        return goalCancelledText;
                    }
                    else if (matchEvent.Detail == "Penalty confirmed")
                    {
                        string penaltyText = $"Le acaban de cobrar un penal a favor de {matchEvent.Team.Name} a los {matchEvent.Time.Elapsed}";
                        if (matchEvent.Time.extra == null)
                        {
                            penaltyText += ".\n";
                        }
                        else penaltyText += $" + {matchEvent.Time.extra} de tiempo Adicional.\n";
                        return penaltyText;
                    }
                    break;
            }
            return $"Evento inesperado de tipo {matchEvent.Type} y detalle {matchEvent.Detail}.";
        }
    }
}
