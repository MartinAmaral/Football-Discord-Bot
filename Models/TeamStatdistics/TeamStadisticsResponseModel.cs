namespace Models.TeamStatdistics
{
#pragma warning disable CS8618
    public class TeamStadisticsResponseModel
    {
        public FixtureModel Fixtures { get; set; }
        public GoalsModels Goals { get; set; }
        public PenaltyModel Penalty { get; set; }
        public CardsModels Cards { get; set; }
    }
}
