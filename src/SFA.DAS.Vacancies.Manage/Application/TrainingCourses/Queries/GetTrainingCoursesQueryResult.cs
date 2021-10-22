using System.Collections.Generic;
using SFA.DAS.Vacancies.Manage.InnerApi.Responses;

namespace SFA.DAS.Vacancies.Manage.Application.TrainingCourses.Queries
{
    public class GetTrainingCoursesQueryResult
    {
        public IEnumerable<GetStandardsListItem> TrainingCourses { get ; set ; }
    }
}