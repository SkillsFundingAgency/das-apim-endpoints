using SFA.DAS.ProviderRequestApprenticeTraining.InnerApi.Responses;
using System.Collections.Generic;

namespace SFA.DAS.ProviderRequestApprenticeTraining.Application.Queries.GetAggregatedEmployerRequests
{
    public class GetAggregatedEmployerRequestsResult
    {
        public IEnumerable<GetAggregatedEmployerRequestsResponse> AggregatedEmployerRequests { get; set; }
    }
}
