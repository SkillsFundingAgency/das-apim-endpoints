using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.ProviderRequestApprenticeTraining.InnerApi.Requests
{
    public class GetAggregatedEmployerRequestsRequest : IGetApiRequest
    {
        public long Ukprn { get; set; }

        public GetAggregatedEmployerRequestsRequest(long ukprn)
        { 
            Ukprn = ukprn;
        }

        public string GetUrl => $"api/providers/{Ukprn}/employer-requests/aggregated";
    }
}
