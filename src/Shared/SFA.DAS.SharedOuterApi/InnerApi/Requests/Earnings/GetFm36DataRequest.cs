using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.SharedOuterApi.InnerApi.Requests.Earnings
{
    public class GetFm36DataRequest : IGetApiRequest
    {
        public long Ukprn { get; }
        public int CollectionYear { get; set; }
        public byte CollectionPeriod { get; set; }

        public string GetUrl => $"{Ukprn}/fm36/{CollectionYear}/{CollectionPeriod}";

        public GetFm36DataRequest(long ukprn, int collectionYear, byte collectionPeriod)
        {
            Ukprn = ukprn;
            CollectionYear = collectionYear;
            CollectionPeriod = collectionPeriod;
        }
    }
}