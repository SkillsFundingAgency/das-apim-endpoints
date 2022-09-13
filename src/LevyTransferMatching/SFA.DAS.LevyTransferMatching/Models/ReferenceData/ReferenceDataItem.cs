using System.Text.Json.Serialization;

namespace SFA.DAS.LevyTransferMatching.Models.ReferenceData
{
    public class ReferenceDataItem
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("shortDescription")]//, NullValueHandling = NullValueHandling.Ignore)]
        //TODO for 6.1 and remove global option [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string ShortDescription { get; set; }

        [JsonPropertyName("hint")]//, NullValueHandling = NullValueHandling.Ignore)]
        //TODO for 6.1 and remove global option [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string Hint { get; set; }
    }
}
