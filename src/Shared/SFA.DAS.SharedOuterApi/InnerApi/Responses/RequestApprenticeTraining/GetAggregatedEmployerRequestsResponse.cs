using SFA.DAS.SharedOuterApi.InnerApi.Requests.RequestApprenticeTraining;
using System;
using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.SharedOuterApi.InnerApi.Responses.RequestApprenticeTraining
{
    [ExcludeFromCodeCoverage]
    public class GetAggregatedEmployerRequestsResponse 
    {
        public Guid EmployerRequestId { get; set; }
        public string StandardReference { get; set; }
        public string StandardTitle { get; set; }
        public int StandardLevel { get; set; }
        public DateTime RequestedAt { get; set; }
        public DateTime ExpiryAt { get; set; }
        public RequestStatus RequestStatus { get; set; }
        public int NumberOfResponses { get; set; }
        public int NewNumberOfResponses { get; set; }
    }
}
    