using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Approvals.InnerApi.Requests
{
    public class GetProviderRequest : IGetApiRequest
    {
        public int Ukprn { get; }
        public string GetUrl => $"providers/{Ukprn}";
        public GetProviderRequest(int ukprn)
        {
            Ukprn = ukprn;
        }
    }
}