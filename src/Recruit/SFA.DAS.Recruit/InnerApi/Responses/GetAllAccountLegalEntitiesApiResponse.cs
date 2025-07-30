using Newtonsoft.Json;
using System.Collections.Generic;

namespace SFA.DAS.Recruit.InnerApi.Responses
{
    public record GetAllAccountLegalEntitiesApiResponse
    {
        [JsonProperty("pageInfo")]
        public PageInfo PageInfo { get; set; }

        [JsonProperty("legalEntities")]
        public List<GetAccountLegalEntityResponseItem> LegalEntities { get; set; }
    }
}
