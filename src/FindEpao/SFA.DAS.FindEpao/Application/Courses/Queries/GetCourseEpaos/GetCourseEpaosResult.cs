using System.Collections.Generic;
using SFA.DAS.FindEpao.InnerApi.Responses;

namespace SFA.DAS.FindEpao.Application.Courses.Queries.GetCourseEpaos
{
    public class GetCourseEpaosResult
    {
        public GetStandardsListItem Course { get; set; }
        public IEnumerable<GetCourseEpaoListItem> Epaos { get; set; }
    }
}