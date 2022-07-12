using System.Collections.Generic;
using Newtonsoft.Json;

namespace SFA.DAS.ApimDeveloper.InnerApi.Responses
{
    public class GetApiProductSubscriptionsResponse
    {
        [JsonProperty("subscriptions")]
        public IEnumerable<GetApiProductSubscriptionsResponseItem> Subscriptions { get; set; }
    }

    public class GetApiProductSubscriptionsResponseItem
    {
        [JsonProperty("key")]
        public string Key { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
    }
}