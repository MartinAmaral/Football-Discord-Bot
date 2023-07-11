namespace Models.Fixture
{
    public class FixtureModel
    {
        public int Id { get; set; }
        public string Date { get; set; }
        public VenueModel Venue { get; set; }
        public StatusModel Status { get; set; }
    }
}
