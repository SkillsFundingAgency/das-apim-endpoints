using Newtonsoft.Json;

namespace SFA.DAS.LevyTransferMatching.Models.ReferenceData
{
    public class ReferenceDataItem
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("shortDescription", NullValueHandling = NullValueHandling.Ignore)]
        public string ShortDescription { get; set; }

        [JsonProperty("hint", NullValueHandling = NullValueHandling.Ignore)]
        public string Hint { get; set; }
    }
}
