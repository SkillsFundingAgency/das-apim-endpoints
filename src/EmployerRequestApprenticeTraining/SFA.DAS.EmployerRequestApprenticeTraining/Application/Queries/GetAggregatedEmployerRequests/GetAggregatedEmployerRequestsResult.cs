using SFA.DAS.EmployerRequestApprenticeTraining.InnerApi.Responses;
using System.Collections.Generic;

namespace SFA.DAS.EmployerRequestApprenticeTraining.Application.Queries.GetAggregatedEmployerRequests
{
    public class GetAggregatedEmployerRequestsResult
    {
        public IEnumerable<GetAggregatedEmployerRequestsResponse> AggregatedEmployerRequests { get; set; }
    }
}
