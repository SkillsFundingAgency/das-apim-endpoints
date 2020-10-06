using System.Collections.Generic;
using SFA.DAS.FindEpao.InnerApi.Responses;

namespace SFA.DAS.FindEpao.Application.Courses.Queries.GetCourseList
{
    public class GetCourseListResult
    {
        public IEnumerable<GetStandardsListItem> Courses { get ; set ; }
    }
}