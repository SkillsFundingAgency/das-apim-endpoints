using System.Collections.Generic;
using Newtonsoft.Json;

namespace SFA.DAS.SharedOuterApi.InnerApi.Responses
{
    public class GetProviderAccountLegalEntitiesResponse
    {
        [JsonProperty("AccountProviderLegalEntities")]
        public List<GetProviderAccountLegalEntityItem> AccountProviderLegalEntities { get; set; }
    }

    public class GetProviderAccountLegalEntityItem
    {
        [JsonProperty("AccountId")]
        public long AccountId { get; set; }

        [JsonProperty("AccountPublicHashedId")]
        public string AccountPublicHashedId { get; set; }

        [JsonProperty("AccountHashedId")]
        public string AccountHashedId { get; set; }

        [JsonProperty("AccountName")]
        public string AccountName { get; set; }

        [JsonProperty("AccountLegalEntityId")]
        public long AccountLegalEntityId { get; set; }

        [JsonProperty("AccountLegalEntityPublicHashedId")]
        public string AccountLegalEntityPublicHashedId { get; set; }

        [JsonProperty("AccountLegalEntityName")]
        public string AccountLegalEntityName { get; set; }

        [JsonProperty("AccountProviderId")]
        public long AccountProviderId { get; set; }
    }
}