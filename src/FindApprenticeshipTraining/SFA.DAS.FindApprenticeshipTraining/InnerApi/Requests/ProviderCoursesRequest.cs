using SFA.DAS.SharedOuterApi.Common;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindApprenticeshipTraining.InnerApi.Requests;

public sealed class ProviderCoursesRequest : IGetApiRequest
{
    private long Ukrpn { get; }
    public string Version => ApiVersionNumber.Two;
    public string GetUrl => $"api/providers/{Ukrpn}/courses";

    public ProviderCoursesRequest(long ukprn)
    {
        Ukrpn = ukprn;
    }
}
