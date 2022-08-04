using System.Collections.Generic;

namespace SFA.DAS.RoatpCourseManagement.Application.Standards.Queries.GetAvailableCoursesForProvider
{
    public class GetAvailableCoursesForProviderQueryResult
    {
        public List<AvailableCourseModel> AvailableCourses { get; set; } = new List<AvailableCourseModel>();
    }
}
