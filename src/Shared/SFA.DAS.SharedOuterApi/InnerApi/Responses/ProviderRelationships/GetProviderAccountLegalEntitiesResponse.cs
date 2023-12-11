using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace SFA.DAS.SharedOuterApi.InnerApi.Responses
{
    public class GetProviderAccountLegalEntitiesResponse
    {
        [JsonPropertyName("AccountProviderLegalEntities")]
        public List<GetProviderAccountLegalEntityItem> AccountProviderLegalEntities { get; set; }
    }

    public class GetProviderAccountLegalEntityItem
    {
        [JsonPropertyName("AccountId")]
        public long AccountId { get; set; }

        [JsonPropertyName("AccountPublicHashedId")]
        public string AccountPublicHashedId { get; set; }

        [JsonPropertyName("AccountHashedId")]
        public string AccountHashedId { get; set; }

        [JsonPropertyName("AccountName")]
        public string AccountName { get; set; }

        [JsonPropertyName("AccountLegalEntityId")]
        public long AccountLegalEntityId { get; set; }

        [JsonPropertyName("AccountLegalEntityPublicHashedId")]
        public string AccountLegalEntityPublicHashedId { get; set; }

        [JsonPropertyName("AccountLegalEntityName")]
        public string AccountLegalEntityName { get; set; }

        [JsonPropertyName("AccountProviderId")]
        public long AccountProviderId { get; set; }
    }
}