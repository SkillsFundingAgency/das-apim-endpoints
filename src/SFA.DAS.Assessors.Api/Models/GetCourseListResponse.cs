using System.Collections.Generic;

namespace SFA.DAS.Assessors.Api.Models
{
    public class GetCourseListResponse
    {
        public IEnumerable<GetCourseListItem> Courses { get; set; }
    }
}