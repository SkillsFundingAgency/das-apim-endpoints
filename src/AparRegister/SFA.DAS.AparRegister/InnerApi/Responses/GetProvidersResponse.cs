using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace SFA.DAS.AparRegister.InnerApi.Responses
{
    public class GetProvidersResponse
    {
        [JsonPropertyName("registeredProviders")]
        public IList<RegisteredProvider> RegisteredProviders { get; set; }
    }

    public partial class RegisteredProvider
    {
        [JsonPropertyName("ukprn")]
        public int Ukprn { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("tradingName")]
        public string TradingName { get; set; }

        [JsonPropertyName("providerTypeId")]
        public long ProviderTypeId { get; set; }

        [JsonPropertyName("statusId")]
        public long StatusId { get; set; }

        [JsonPropertyName("canAccessApprenticeshipService")]
        public bool CanAccessApprenticeshipService { get; set; }

    }
}