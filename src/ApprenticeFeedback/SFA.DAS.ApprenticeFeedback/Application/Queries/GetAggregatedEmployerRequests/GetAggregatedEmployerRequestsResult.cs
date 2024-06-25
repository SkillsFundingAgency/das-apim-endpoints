using SFA.DAS.ApprenticeFeedback.Models;
using System.Collections.Generic;

namespace SFA.DAS.ApprenticeFeedback.Application.Queries.GetAggregatedEmployerRequests
{
    public class GetAggregatedEmployerRequestsResult
    {
        public IEnumerable<AggregatedEmployerRequest> AggregatedEmployerRequests { get; set; }
    }
}
