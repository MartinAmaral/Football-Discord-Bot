namespace Models.LeagueStandings
{
    public class MatchesModel
    {
        public int Played { get; set; }
        public int Win { get; set; }
        public int Draw { get; set; }
        public int Lose { get; set; }
        public GoalsModel Goals { get; set; }
    }
}
