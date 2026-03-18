using System.Collections.Generic;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Responses.ProviderRelationships;

namespace SFA.DAS.VacanciesManage.Application.Providers.Queries.GetProviderAccountLegalEntities
{
    public class GetProviderAccountLegalEntitiesQueryResponse
    {
        public List<GetProviderAccountLegalEntityItem> ProviderAccountLegalEntities { get ; set ; }
    }
}