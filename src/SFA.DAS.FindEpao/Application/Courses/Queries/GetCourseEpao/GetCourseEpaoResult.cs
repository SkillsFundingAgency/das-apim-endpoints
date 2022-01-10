using System;
using System.Collections.Generic;
using SFA.DAS.FindEpao.InnerApi.Responses;

namespace SFA.DAS.FindEpao.Application.Courses.Queries.GetCourseEpao
{
    public class GetCourseEpaoResult
    {
        public GetEpaoResponse Epao { get; set; }
        public GetStandardsListItem Course { get; set; }
        public IEnumerable<GetStandardsExtendedListItem> StandardVersions { get; set; }
        public int CourseEpaosCount { get; set; }
        public DateTime EffectiveFrom { get; set; }
        public IEnumerable<EpaoDeliveryArea> EpaoDeliveryAreas { get; set; }
        public IEnumerable<GetDeliveryAreaListItem> DeliveryAreas { get; set; }
        public IEnumerable<GetStandardsListItem> AllCourses { get; set; }
    }
}