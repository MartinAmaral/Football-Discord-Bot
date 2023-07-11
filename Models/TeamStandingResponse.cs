using Models.TeamStanding;

namespace Models
{
    public class TeamStandingResponse
    {
        public int Results { get; set; }
        public TeamStandingResponseModel[] Response { get; set; }
    }
}
