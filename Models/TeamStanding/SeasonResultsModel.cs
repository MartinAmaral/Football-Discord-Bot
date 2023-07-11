namespace Models.TeamStanding
{
    public class SeasonResultsModel
    {
#pragma warning disable CS8618

        public int Played { get; set; }
        public int Win { get; set; }
        public int Draw { get; set; }
        public int Lose { get; set; }
        public GoalsModel Goals { get; set; }
    }
}
