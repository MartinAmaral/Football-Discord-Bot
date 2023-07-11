using Newtonsoft.Json;

namespace Models.TeamStatdistics
{
    public class CardTypeModel
    {
#pragma warning disable

        [JsonProperty("0-15")]
        public CardTimeModel t1 { get; set; }
        [JsonProperty("16-30")]
        public CardTimeModel t2 { get; set; }
        [JsonProperty("31-45")]
        public CardTimeModel t3 { get; set; }
        [JsonProperty("46-60")]
        public CardTimeModel t4 { get; set; }
        [JsonProperty("61-75")]
        public CardTimeModel t5 { get; set; }
        [JsonProperty("76-90")]
        public CardTimeModel t6 { get; set; }
        [JsonProperty("91-105")]
        public CardTimeModel t7 { get; set; }
        [JsonProperty("106-120")]
        public CardTimeModel t8final { get; set; }


    }
}
