using SFA.DAS.SharedOuterApi.Interfaces;
using System;

namespace SFA.DAS.ProviderRequestApprenticeTraining.InnerApi.Requests
{
    public class GetSelectEmployerRequestsRequest : IGetApiRequest
    {
        public string GetUrl => $"api/employerrequest/provider/{Ukprn}/selectrequests/{StandardReference}";
        public string StandardReference { get; set; }
        public long Ukprn { get; set; }

        public GetSelectEmployerRequestsRequest(string standardReference, long ukprn)
        {
            StandardReference = standardReference;
            Ukprn = ukprn;
        }
    }
}
