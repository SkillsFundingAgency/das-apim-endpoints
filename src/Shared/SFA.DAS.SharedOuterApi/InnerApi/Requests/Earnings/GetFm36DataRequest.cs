using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.SharedOuterApi.InnerApi.Requests.Earnings
{
    public class GetFm36DataRequest : IGetApiRequest
    {
        public long Ukprn { get; }

        public string GetUrl => $"{Ukprn}/fm36";

        public GetFm36DataRequest(long ukprn)
        {
            Ukprn = ukprn;
        }
    }
}
