using Discord.WebSocket;
using Models.Events;

namespace Discord_Bot
{
    public class FollowedMatch
    {
        public int FixtureID { get; set; }
        public List<EventModel> storedEvents = new();
        public string StartingDate { get; set; }
        public Dictionary<SocketInteraction, HashSet<string>> channelsUser = new();

        public FollowedMatch(int fixture, string date, SocketSlashCommand command)
        {
            FixtureID = fixture;
            StartingDate = date;
            channelsUser.Add(command, new HashSet<string> { command.User.Mention });
        }

    }
}
