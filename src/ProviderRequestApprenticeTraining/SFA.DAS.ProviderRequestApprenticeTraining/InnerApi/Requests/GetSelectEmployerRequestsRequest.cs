using SFA.DAS.SharedOuterApi.Interfaces;
using System;

namespace SFA.DAS.ProviderRequestApprenticeTraining.InnerApi.Requests
{
    public class GetSelectEmployerRequestsRequest : IGetApiRequest
    {
        public string GetUrl => $"api/employerrequest/select-employer-requests?standardReference={StandardReference}&ukprn={Ukprn}";
        public string StandardReference { get; set; }
        public int Ukprn { get; set; }

        public GetSelectEmployerRequestsRequest(string standardReference, int ukprn)
        {
            StandardReference = standardReference;
            Ukprn = ukprn;
        }
    }
}
