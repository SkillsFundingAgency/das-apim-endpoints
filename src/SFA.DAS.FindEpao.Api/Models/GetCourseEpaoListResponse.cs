using System.Collections.Generic;

namespace SFA.DAS.FindEpao.Api.Models
{
    public class GetCourseEpaoListResponse
    {
        public GetCourseListItem Course { get; set; }
        public IEnumerable<GetCourseEpaoListItem> Epaos { get; set; }
    }
}