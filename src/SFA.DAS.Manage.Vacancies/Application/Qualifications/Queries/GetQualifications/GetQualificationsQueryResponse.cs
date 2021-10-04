using System.Collections.Generic;
using SFA.DAS.Vacancies.Manage.InnerApi.Responses;

namespace SFA.DAS.Vacancies.Manage.Application.Qualifications.Queries.GetQualifications
{
    public class GetQualificationsQueryResponse
    {
        public List<GetQualificationsItem> Qualifications { get ; set ; }
    }
}