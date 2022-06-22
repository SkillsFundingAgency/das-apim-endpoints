using System.Collections.Generic;
using SFA.DAS.VacanciesManage.InnerApi.Responses;

namespace SFA.DAS.VacanciesManage.Application.TrainingCourses.Queries
{
    public class GetTrainingCoursesQueryResult
    {
        public IEnumerable<GetStandardsListItem> TrainingCourses { get ; set ; }
    }
}