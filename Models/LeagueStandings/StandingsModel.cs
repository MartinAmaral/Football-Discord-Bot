using Newtonsoft.Json;

namespace Models.LeagueStandings
{
    public class StandingsModel
    {
        public int Rank { get; set; }
        public TeamModel Team { get; set; }

        public int Points { get; set; }

        public int GoalsDiff { get; set; }
        [JsonProperty("all")]
        public MatchesModel Matches { get; set; }


    }
}
