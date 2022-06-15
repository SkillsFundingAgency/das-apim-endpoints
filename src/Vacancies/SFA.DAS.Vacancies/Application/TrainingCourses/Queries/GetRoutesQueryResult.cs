using System.Collections.Generic;
using SFA.DAS.Vacancies.InnerApi.Responses;

namespace SFA.DAS.Vacancies.Application.TrainingCourses.Queries
{
    public class GetRoutesQueryResult
    {
        public IEnumerable<GetRoutesListItem> Routes { get ; set ; }
    }
}