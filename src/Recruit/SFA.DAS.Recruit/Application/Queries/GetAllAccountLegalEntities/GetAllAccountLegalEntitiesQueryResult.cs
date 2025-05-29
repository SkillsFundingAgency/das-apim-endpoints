using SFA.DAS.Recruit.InnerApi.Responses;
using System.Collections.Generic;

namespace SFA.DAS.Recruit.Application.Queries.GetAllAccountLegalEntities
{
    public record GetAllAccountLegalEntitiesQueryResult
    {
        public GetAllAccountLegalEntitiesApiResponse.PaginationInfo PageInfo { get; set; }
        public List<GetAccountLegalEntityResponseItem> LegalEntities { get; set; }

        public static implicit operator GetAllAccountLegalEntitiesQueryResult(GetAllAccountLegalEntitiesApiResponse response)
        {
            return new GetAllAccountLegalEntitiesQueryResult
            {
                PageInfo = response.PageInfo,
                LegalEntities = response.LegalEntities
            };
        }
    }
}