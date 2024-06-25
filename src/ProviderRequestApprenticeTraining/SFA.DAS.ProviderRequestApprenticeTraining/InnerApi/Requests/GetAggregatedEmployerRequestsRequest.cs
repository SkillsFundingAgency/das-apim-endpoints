using SFA.DAS.SharedOuterApi.Interfaces;
using System;

namespace SFA.DAS.ProviderRequestApprenticeTraining.InnerApi.Requests
{
    public class GetAggregatedEmployerRequestsRequest : IGetApiRequest
    {
        public string GetUrl => $"api/employerrequest/aggregatedrequests";
    }
}
