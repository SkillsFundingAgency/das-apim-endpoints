using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.SharedOuterApi.InnerApi.Requests.Learning;

public class GetLearningsRequest : IGetApiRequest
{
    public long Ukprn { get; set; }
    public int CollectionYear { get; set; }
    public byte CollectionPeriod { get; set; }
    public string GetUrl => $"{Ukprn}/{CollectionYear}/{CollectionPeriod}";
}