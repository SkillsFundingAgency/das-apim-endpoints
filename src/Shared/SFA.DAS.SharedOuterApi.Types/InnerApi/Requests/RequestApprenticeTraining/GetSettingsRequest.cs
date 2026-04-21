using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.RequestApprenticeTraining;

public class GetSettingsRequest : IGetApiRequest
{
    public string GetUrl => $"api/settings";
}