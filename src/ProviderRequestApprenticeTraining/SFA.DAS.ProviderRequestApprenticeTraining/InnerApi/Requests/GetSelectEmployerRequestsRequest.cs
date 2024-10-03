using SFA.DAS.SharedOuterApi.Interfaces;
using System;

namespace SFA.DAS.ProviderRequestApprenticeTraining.InnerApi.Requests
{
    public class GetSelectEmployerRequestsRequest : IGetApiRequest
    {
        public string GetUrl => $"api/providers/{Ukprn}/employer-requests/{StandardReference}/select";
        public string StandardReference { get; set; }
        public long Ukprn { get; set; }

        public GetSelectEmployerRequestsRequest(string standardReference, long ukprn)
        {
            StandardReference = standardReference;
            Ukprn = ukprn;
        }
    }
}
