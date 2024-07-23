using SFA.DAS.SharedOuterApi.Interfaces;
using System;

namespace SFA.DAS.ProviderRequestApprenticeTraining.InnerApi.Requests
{
    public class GetAggregatedEmployerRequestsRequest : IGetApiRequest
    {
        public long Ukprn { get; set; }

        public GetAggregatedEmployerRequestsRequest(long ukprn)
        { 
            Ukprn = ukprn;
        }

        public string GetUrl => $"api/employerrequest/{Ukprn}/aggregated";
    }
}
