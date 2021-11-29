using System.Collections.Generic;
using Newtonsoft.Json;

namespace SFA.DAS.ApimDeveloper.InnerApi.Responses
{
    public class GetAvailableApiProductsResponse
    {
        [JsonProperty("products")]
        public IEnumerable<GetAvailableApiProductItem> Products { get; set; }
    }

    public class GetAvailableApiProductItem
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("displayName")]
        public string DisplayName { get; set; }
        [JsonProperty("description")]
        public string Description { get; set; }
        [JsonProperty("documentation")]
        public string Documentation { get; set; }
    }
}