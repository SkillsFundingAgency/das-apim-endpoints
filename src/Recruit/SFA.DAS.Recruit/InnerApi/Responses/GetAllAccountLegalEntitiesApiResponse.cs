using Newtonsoft.Json;
using System.Collections.Generic;

namespace SFA.DAS.Recruit.InnerApi.Responses
{
    public record GetAllAccountLegalEntitiesApiResponse
    {
        [JsonProperty("pageInfo")]
        public PaginationInfo PageInfo { get; set; }

        [JsonProperty("legalEntities")]
        public List<GetAccountLegalEntityResponseItem> LegalEntities { get; set; }

        public class PaginationInfo
        {
            [JsonProperty("totalCount")]
            public int TotalCount { get; set; }

            [JsonProperty("pageIndex")]
            public int PageIndex { get; set; }

            [JsonProperty("pageSize")]
            public int PageSize { get; set; }

            [JsonProperty("totalPages")]
            public int TotalPages { get; set; }

            [JsonProperty("hasPreviousPage")]
            public bool HasPreviousPage { get; set; }

            [JsonProperty("hasNextPage")]
            public bool HasNextPage { get; set; }
        }
    }
}
