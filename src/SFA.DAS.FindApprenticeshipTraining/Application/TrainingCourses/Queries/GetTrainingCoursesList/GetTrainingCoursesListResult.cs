using System.Collections.Generic;
using SFA.DAS.FindApprenticeshipTraining.Application.Domain.InnerApi.Responses;

namespace SFA.DAS.FindApprenticeshipTraining.Application.Application.TrainingCourses.Queries.GetTrainingCoursesList
{
    public class GetTrainingCoursesListResult
    {
        public IEnumerable<GetStandardsListItem> Courses { get; set; }
    }
}
