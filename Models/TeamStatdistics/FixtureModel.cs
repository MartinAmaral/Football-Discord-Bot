namespace Models.TeamStatdistics
{
#pragma warning disable CS8618

    public class FixtureModel
    {
        public PlaceOutcomeModel Played { get; set; }
        public PlaceOutcomeModel Wins { get; set; }
        public PlaceOutcomeModel Draws { get; set; }
        public PlaceOutcomeModel Loses { get; set; }
    }
}
