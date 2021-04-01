using System.Collections.Generic;

namespace SFA.DAS.EmployerDemand.Api.Models
{
    public class GetAggregatedCourseDemandListResponse
    {
        public IEnumerable<GetCourseListItem> Courses { get; set; }
        public IEnumerable<GetAggregatedCourseDemandSummary> AggregatedCourseDemands { get; set; }
        public int TotalFiltered { get ; set ; }
        public int Total { get ; set ; }
    }
}