using System.Collections.Generic;
using SFA.DAS.Reservations.InnerApi.Responses;

namespace SFA.DAS.Reservations.Application.TrainingCourses.Queries.GetTrainingCourseList
{
    public class GetTrainingCoursesResult
    {
        public IEnumerable<TrainingCourseListItem> Courses { get; set; }
    }
}