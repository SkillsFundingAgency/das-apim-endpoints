using System.Collections.Generic;

namespace SFA.DAS.Forecasting.Api.Model
{
    public class GetAllActiveCoursesResponse
    {
        public IEnumerable<ApprenticeshipCourse> ApprenticeshipCourses { get; set; }
    }
}