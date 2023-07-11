using Models.TopScorers;

namespace Models
{
    public class TopScorersResponse
    {
        public int Results { get; set; }
        public TopScorersModel[] Response { get; set; }
    }
}
