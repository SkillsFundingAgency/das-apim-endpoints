using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.RoatpV2;

public class GetRoatpProviderRequest(int ukprn) : IGetApiRequest
{
    public string GetUrl => $"api/providers/{ukprn}";
}