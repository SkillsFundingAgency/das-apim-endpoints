using System.Collections.Generic;
using Newtonsoft.Json;

namespace SFA.DAS.Vacancies.InnerApi.Responses
{
    public class GetProviderAccountLegalEntitiesResponse
    {
        [JsonProperty("AccountProviderLegalEntities")]
        public List<GetProviderAccountLegalEntityItem> AccountProviderLegalEntities { get; set; }
    }

    public class GetProviderAccountLegalEntityItem
    {
        [JsonProperty("AccountLegalEntityPublicHashedId")]
        public string AccountLegalEntityPublicHashedId { get; set; }

    }
}