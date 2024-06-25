
using SFA.DAS.ProviderRequestApprenticeTraining.Models;
using System.Collections.Generic;

namespace SFA.DAS.ProviderRequestApprenticeTraining.Application.Queries.GetAggregatedEmployerRequests
{
    public class GetAggregatedEmployerRequestsResult
    {
        public IEnumerable<AggregatedEmployerRequest> AggregatedEmployerRequests { get; set; }
    }
}
