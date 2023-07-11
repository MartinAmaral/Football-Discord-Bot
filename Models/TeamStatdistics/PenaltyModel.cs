namespace Models.TeamStatdistics
{
#pragma warning disable CS8618

    public class PenaltyModel
    {
        public PenaltyStatsModel Scored { get; set; }
        public PenaltyStatsModel Missed { get; set; }
        public int Total { get; set; }
    }
}
