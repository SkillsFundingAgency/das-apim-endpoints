using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.LevyTransferMatching;

public class GetApplicationsToAutoDeclineRequest() : IGetApiRequest
{
    public string GetUrl => "/applications-auto-decline";
}