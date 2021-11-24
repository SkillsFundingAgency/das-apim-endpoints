using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.SharedOuterApi.InnerApi.Requests
{
    public class GetProviderAccountLegalEntitiesRequest : IGetApiRequest
    {
        private readonly int? _ukprn;

        public GetProviderAccountLegalEntitiesRequest(int? ukprn)
        {
            _ukprn = ukprn;
        }

        public string GetUrl => $"accountproviderlegalentities?ukprn={_ukprn}&operations=1&operations=2";
    }
}