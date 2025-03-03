using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.SharedOuterApi.InnerApi.Requests.LevyTransferMatching;

public class GetApplicationsToAutoExpireRequest() : IGetApiRequest
{
    public string GetUrl => "/applications-auto-expire";
}