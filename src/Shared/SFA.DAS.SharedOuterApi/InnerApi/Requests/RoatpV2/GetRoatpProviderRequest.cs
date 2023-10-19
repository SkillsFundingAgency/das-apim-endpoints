using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.SharedOuterApi.InnerApi.Requests.RoatpV2
{
    public class GetRoatpProviderRequest : IGetApiRequest
    {
        private readonly int _ukprn;

        public GetRoatpProviderRequest(int ukprn)
        {
            _ukprn = ukprn;
        }

        public string GetUrl => $"api/providers/{_ukprn}";
    }
}