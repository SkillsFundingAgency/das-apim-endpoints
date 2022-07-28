using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace SFA.DAS.ApimDeveloper.InnerApi.Responses
{
    public class GetApiProductSubscriptionsResponse
    {
        [JsonPropertyName("subscriptions")]
        public IEnumerable<GetApiProductSubscriptionsResponseItem> Subscriptions { get; set; }
    }

    public class GetApiProductSubscriptionsResponseItem
    {
        [JsonPropertyName("key")]
        public string Key { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }
    }
}