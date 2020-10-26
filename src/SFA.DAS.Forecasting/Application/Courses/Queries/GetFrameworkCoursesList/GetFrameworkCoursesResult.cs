using System.Collections.Generic;
using SFA.DAS.Forecasting.InnerApi.Responses;

namespace SFA.DAS.Forecasting.Application.Courses.Queries.GetFrameworkCoursesList
{
    public class GetFrameworkCoursesResult
    {
        public IEnumerable<GetFrameworksListItem> Frameworks { get; set; }
    }
}