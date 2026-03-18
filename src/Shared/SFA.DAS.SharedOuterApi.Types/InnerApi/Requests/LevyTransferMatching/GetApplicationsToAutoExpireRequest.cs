using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.LevyTransferMatching;

public class GetApplicationsToAutoExpireRequest() : IGetApiRequest
{
    public string GetUrl => "/applications-auto-expire";
}