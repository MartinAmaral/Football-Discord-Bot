using Newtonsoft.Json;

namespace Models.Fixture
{
    public class StatusModel
    {
        [JsonProperty("long")]
        public string? MatchStatus { get; set; }
        public int? Elapsed { get; set; }
    }
}
