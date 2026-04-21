using SFA.DAS.Apim.Shared.Interfaces;

using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.LevyTransferMatching;

public class GetApplicationsToAutoExpireRequest() : IGetApiRequest
{
    public string GetUrl => "/applications-auto-expire";
}