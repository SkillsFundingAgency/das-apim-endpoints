using System.Collections.Generic;

namespace SFA.DAS.FindApprenticeshipTraining.Api.Models
{
    public class GetTrainingCoursesListResponse
    {
        public IEnumerable<GetTrainingCourseListItem> TrainingCourses { get; set; }
    }
}
