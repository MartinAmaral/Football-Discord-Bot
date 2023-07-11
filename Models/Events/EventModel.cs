namespace Models.Events
{
    public class EventModel
    {

        public TimeModel Time { get; set; }
        public TeamModel Team { get; set; }
        public PlayerModel Player { get; set; }
        public PlayerModel Assist { get; set; }
        public string Type { get; set; }
        public string Detail { get; set; }
    }
}
