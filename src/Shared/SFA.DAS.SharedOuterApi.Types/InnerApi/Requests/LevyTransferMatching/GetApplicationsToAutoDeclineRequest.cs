using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.LevyTransferMatching;

public class GetApplicationsToAutoDeclineRequest() : IGetApiRequest
{
    public string GetUrl => "/applications-auto-decline";
}