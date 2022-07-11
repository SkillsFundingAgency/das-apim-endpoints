using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace SFA.DAS.ApimDeveloper.InnerApi.Responses
{
    public class GetAvailableApiProductsResponse
    {
        [JsonPropertyName("products")]
        public IEnumerable<GetAvailableApiProductItem> Products { get; set; }
    }

    public class GetAvailableApiProductItem
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("displayName")]
        public string DisplayName { get; set; }
        [JsonPropertyName("description")]
        public string Description { get; set; }
        [JsonPropertyName("documentation")]
        public string Documentation { get; set; }
    }
}