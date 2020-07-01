using System.Collections.Generic;
using SFA.DAS.FindApprenticeshipTraining.Application.InnerApi.Responses;

namespace SFA.DAS.FindApprenticeshipTraining.Application.Application.TrainingCourses.Queries.GetTrainingCoursesList
{
    public class GetTrainingCoursesListResult
    {
        public IEnumerable<GetStandardsListItem> Courses { get; set; }
        public int Total { get ; set ; }
        public int TotalFiltered { get ; set ; }
    }
}
