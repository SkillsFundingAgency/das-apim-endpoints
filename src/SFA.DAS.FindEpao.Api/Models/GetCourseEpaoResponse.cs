using System;
using System.Collections.Generic;

namespace SFA.DAS.FindEpao.Api.Models
{
    public class GetCourseEpaoResponse
    {
        public EpaoDetails Epao { get; set; }
        public GetCourseListItem Course { get; set; }
        public IEnumerable<StandardVersions> StandardVersions { get; set; }
        public int CourseEpaosCount { get; set; }
        public DateTime EffectiveFrom { get; set; }
        public IEnumerable<EpaoDeliveryArea> EpaoDeliveryAreas { get; set; }
        public IEnumerable<GetDeliveryAreaListItem> DeliveryAreas { get; set; }
        public IEnumerable<GetAllCoursesListItem> AllCourses { get; set; }
    }
}