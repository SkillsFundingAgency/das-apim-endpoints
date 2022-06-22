using System.Collections.Generic;

namespace SFA.DAS.FindEpao.Api.Models
{
    public class GetCourseListResponse
    {
        public IEnumerable<GetCourseListItem> Courses { get; set; }
    }
}