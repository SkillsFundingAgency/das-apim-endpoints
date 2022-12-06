using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Funding.InnerApi.Requests.ProviderEarnings
{
    public class GetProviderEarningsSummaryRequest : IGetApiRequest
    {
        private readonly long _ukprn;

        public GetProviderEarningsSummaryRequest(long ukprn)
        {
            _ukprn = ukprn;
        }

        public string GetUrl => $"{_ukprn}/summary";
    }
}