using System.Collections.Generic;
using SFA.DAS.Vacancies.Manage.InnerApi.Responses;

namespace SFA.DAS.Vacancies.Manage.Application.Providers.Queries.GetProviderAccountLegalEntities
{
    public class GetQualificationsQueryResponse
    {
        public List<GetQualificationsItem> ProviderAccountLegalEntities { get ; set ; }
    }
}