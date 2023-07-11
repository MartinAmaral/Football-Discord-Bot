using Models.LeagueStandings;

namespace Models
{
    public class LeagueStandingsResponse
    {
#pragma warning disable
        public int Results { get; set; }
        public ResponseModel[] response { get; set; }
    }
}
