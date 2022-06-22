using System.Collections.Generic;

namespace SFA.DAS.EmployerDemand.InnerApi.Responses
{
    public class GetAggregatedCourseDemandListResponse
    {
        public IEnumerable<GetAggreatedCourseDemandSummaryResponse> AggregatedCourseDemandList { get; set; }
        public int TotalFiltered { get ; set ; }
        public int Total { get ; set ; }
        public List<string> Sectors { get; set; }
    }
}