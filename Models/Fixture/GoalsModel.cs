using Newtonsoft.Json;

namespace Models.Fixture
{
    public class GoalsModel
    {
        [JsonProperty("home")]
        public int? HomeTotalGoals { get; set; }
        [JsonProperty("Away")]
        public int? AwayTotalsGoals { get; set; }

    }
}
