using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.SharedOuterApi.InnerApi.Requests.ProviderRelationships;
public class GetCoursesForProviderRequest(long ukprn) : IGetApiRequest
{
    public string Version => "2.0";

    public long Ukprn { get; set; } = ukprn;

    public string GetUrl => $"api/provider-courses-timeline/{Ukprn}";
}
