using System.Collections.Generic;
using Newtonsoft.Json;

namespace SFA.DAS.Vacancies.Manage.InnerApi.Responses
{
    public class GetQualificationsResponse
    {
        [JsonProperty("Qualifications")]
        public List<GetQualificationsItem> Qualifications { get; set; }
    }

    public class GetProviderAccountLegalEntityItem
    {
        [JsonProperty("AccountId")]
        public long AccountId { get; set; }

        [JsonProperty("AccountPublicHashedId")]
        public string AccountPublicHashedId { get; set; }

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