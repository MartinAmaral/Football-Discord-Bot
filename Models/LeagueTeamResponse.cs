using Models.LeaguesTeam;

namespace Models
{
    public class LeagueTeamResponse
    {
        public int Results { get; set; }
        public LeaguesModel[] Response { get; set; }
    }
}
