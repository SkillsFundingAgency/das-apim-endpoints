using System.Collections.Generic;
using SFA.DAS.Forecasting.InnerApi.Responses;

namespace SFA.DAS.Forecasting.Application.Courses.Queries.GetStandardCoursesList
{
    public class GetStandardCoursesResult
    {
        public IEnumerable<GetStandardsListItem> Standards { get; set; }
    }
}