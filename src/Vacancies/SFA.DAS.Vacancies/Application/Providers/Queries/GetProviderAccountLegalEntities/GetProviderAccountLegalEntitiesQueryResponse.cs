using System.Collections.Generic;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Responses.Courses;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Responses.ProviderRelationships;

namespace SFA.DAS.Vacancies.Application.Providers.Queries.GetProviderAccountLegalEntities
{
    public class GetProviderAccountLegalEntitiesQueryResponse
    {
        public List<GetProviderAccountLegalEntityItem> ProviderAccountLegalEntities { get ; set ; }
    }
}