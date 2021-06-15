using Newtonsoft.Json;

namespace SFA.DAS.LevyTransferMatching.Models.Tags
{
    public class Tag
    {
        [JsonProperty("tagId")]
        public string TagId { get; set; }
        [JsonProperty("description")]
        public string Description { get; set; }
        [JsonProperty("extendedDescription", NullValueHandling = NullValueHandling.Ignore)]
        public string ExtendedDescription { get; set; }
    }
}
