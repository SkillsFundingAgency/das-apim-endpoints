using System.Collections.Generic;
using SFA.DAS.EmployerDemand.InnerApi.Responses;

namespace SFA.DAS.EmployerDemand.Application.Demand.Queries.GetAggregatedCourseDemandList
{
    public class GetAggregatedCourseDemandListResult
    {
        public IEnumerable<GetStandardsListItem> Courses { get; set; }
        public IEnumerable<GetAggreatedCourseDemandSummaryResponse> AggregatedCourseDemands { get; set; }
        public int TotalFiltered { get ; set ; }
        public int Total { get ; set ; }
    }
}