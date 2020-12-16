using System.Collections.Generic;
using SFA.DAS.FindEpao.InnerApi.Responses;

namespace SFA.DAS.FindEpao.Application.Courses.Queries.GetCourseEpao
{
    public class GetCourseEpaoResult
    {
        public GetEpaoResponse Epao { get; set; }
        public GetStandardsListItem Course { get; set; }
        public int CourseEpaosCount { get; set; }
        public IEnumerable<EpaoDeliveryArea> DeliveryAreas { get; set; }
    }
}