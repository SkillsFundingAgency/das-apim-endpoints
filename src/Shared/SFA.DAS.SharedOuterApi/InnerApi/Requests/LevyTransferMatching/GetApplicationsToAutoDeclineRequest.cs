using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.SharedOuterApi.InnerApi.Requests.LevyTransferMatching;

public class GetApplicationsToAutoDeclineRequest() : IGetApiRequest
{
    public string GetUrl => "/applications-auto-decline";
}