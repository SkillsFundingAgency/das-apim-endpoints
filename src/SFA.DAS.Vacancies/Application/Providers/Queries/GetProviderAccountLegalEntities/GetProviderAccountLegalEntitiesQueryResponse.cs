using System.Collections.Generic;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;

namespace SFA.DAS.Vacancies.Application.Providers.Queries.GetProviderAccountLegalEntities
{
    public class GetProviderAccountLegalEntitiesQueryResponse
    {
        public List<GetProviderAccountLegalEntityItem> ProviderAccountLegalEntities { get ; set ; }
    }
}