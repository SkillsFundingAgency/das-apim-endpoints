using System.Collections.Generic;
using SFA.DAS.Vacancies.Manage.InnerApi.Responses;

namespace SFA.DAS.Vacancies.Manage.Application.Providers.Queries.GetProviderAccountLegalEntities
{
    public class GetProviderAccountLegalEntitiesQueryResponse
    {
        public List<GetProviderAccountLegalEntityItem> ProviderAccountLegalEntities { get ; set ; }
    }
}