using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.TrackProgress.Apis.CommitmentsV2InnerApi;

public class GetProviderRequest : IGetApiRequest
{
    private readonly long _providerId;

    public GetProviderRequest(long providerId)
        => _providerId = providerId;

    public string GetUrl => $"api/providers/{_providerId}";
}