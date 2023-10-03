using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace SFA.DAS.SharedOuterApi.InnerApi.Responses
{
    public abstract class GetAccountProvidersResponse
    {
        [JsonPropertyName("AccountId")]
        public long AccountId { get; set; }
        
        [JsonPropertyName("AccountProviders")]
        public List<AccountProviderResponse> AccountProviders { get; set; }
    }

    public class AccountProviderResponse
    {
        public long Id { get; set; }
        public long ProviderUkprn { get; set; }
        public string ProviderName { get; set; }
    }
}