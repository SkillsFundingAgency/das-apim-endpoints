using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.SharedOuterApi.InnerApi.Requests.RoatpV2;

public class GetRoatpProvidersRequest : IGetApiRequest
{
    public string GetUrl => $"api/providers";
}