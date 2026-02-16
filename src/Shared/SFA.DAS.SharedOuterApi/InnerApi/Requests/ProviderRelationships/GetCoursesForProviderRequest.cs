using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.SharedOuterApi.InnerApi.Requests.ProviderRelationships;
public class GetCoursesForProviderRequest(long ukprn) : IGetApiRequest
{

    public long Ukprn { get; set; } = ukprn;

    public string GetUrl => $"export/providers/{Ukprn}";
}
