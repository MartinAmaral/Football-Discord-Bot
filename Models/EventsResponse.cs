using Models.Events;

namespace Models
{
    public class EventsResponse
    {
        public int Results { get; set; }
        public EventModel[] Response { get; set; }
    }
}
