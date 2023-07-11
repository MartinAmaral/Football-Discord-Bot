namespace Models.TeamStanding
{
    public class StandingModel
    {
#pragma warning disable CS8618

        public int Rank { get; set; }
        public int Points { get; set; }

        public int GoalsDiff { get; set; }
        public SeasonResultsModel All { get; set; }
        public SeasonResultsModel Home { get; set; }
        public SeasonResultsModel Away { get; set; }

    }
}
