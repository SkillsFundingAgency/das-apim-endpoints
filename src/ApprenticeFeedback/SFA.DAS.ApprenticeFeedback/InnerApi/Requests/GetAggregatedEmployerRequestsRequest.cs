using SFA.DAS.SharedOuterApi.Interfaces;
using System;

namespace SFA.DAS.ApprenticeFeedback.InnerApi.Requests
{
    public class GetAggregatedEmployerRequestsRequest : IGetApiRequest
    {
        public string GetUrl => $"api/employerrequest/aggregatedrequests";
    }
}
