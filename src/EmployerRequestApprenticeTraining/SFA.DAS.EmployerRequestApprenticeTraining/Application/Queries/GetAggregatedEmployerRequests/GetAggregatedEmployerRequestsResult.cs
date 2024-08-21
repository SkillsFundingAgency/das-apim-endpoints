using SFA.DAS.SharedOuterApi.InnerApi.Responses.RequestApprenticeTraining;
using System.Collections.Generic;

namespace SFA.DAS.EmployerRequestApprenticeTraining.Application.Queries.GetAggregatedEmployerRequests
{
    public class GetAggregatedEmployerRequestsResult
    {
        public IEnumerable<GetAggregatedEmployerRequestsResponse> AggregatedEmployerRequests { get; set; }
    }
}
