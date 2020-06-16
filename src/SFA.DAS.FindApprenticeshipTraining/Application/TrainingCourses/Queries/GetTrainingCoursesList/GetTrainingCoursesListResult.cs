using System.Collections.Generic;
using SFA.DAS.FindApprenticeshipTraining.Application.Domain.InnerApi.Types;

namespace SFA.DAS.FindApprenticeshipTraining.Application.Application.TrainingCourses.Queries.GetTrainingCoursesList
{
    public class GetTrainingCoursesListResult
    {
        public IEnumerable<GetStandardResponse> Courses { get; set; }
    }
}
