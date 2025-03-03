using System.Collections.Generic;
using System.Text.Json.Serialization;
using SFA.DAS.SharedOuterApi.Models.ProviderRelationships;

namespace SFA.DAS.SharedOuterApi.InnerApi.Responses
{
    public class GetAccountProvidersResponse
    {
        [JsonPropertyName("AccountId")]
        public long AccountId { get; set; }

        [JsonPropertyName("AccountProviders")]
        public List<AccountProviderResponse> AccountProviders { get; set; }
    }

    public class AccountProviderResponse
    {
        public long Ukprn { get; set; }
        public string ProviderName { get; set; }
        public List<AccountLegalEntityModel> AccountLegalEntities { get; set; } = [];
    }

    public class AccountLegalEntityModel
    {
        public string? PublicHashedId { get; set; }
        public string? Name { get; set; }
        public List<Operation> Operations { get; set; } = [];
    }
}