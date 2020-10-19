using System.Collections.Generic;

namespace SFA.DAS.FindEpao.InnerApi.Responses
{
    public class GetCourseEpaoListResponse
    {
        public IEnumerable<GetCourseEpaoListItem> CourseEpaos { get; set; }
    }
}