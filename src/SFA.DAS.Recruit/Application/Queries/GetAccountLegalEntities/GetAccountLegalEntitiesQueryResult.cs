using System.Collections.Generic;
using SFA.DAS.Recruit.InnerApi.Responses;

namespace SFA.DAS.Recruit.Application.Queries.GetAccountLegalEntities
{
    public class GetAccountLegalEntitiesQueryResult
    {
        public List<GetAccountLegalEntityResponseItem> AccountLegalEntities { get; set; }
        
    }
}