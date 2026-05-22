using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.Roatp;

public class GetRemovedReasonsRequest : IGetApiRequest
{
    public string GetUrl => "removed-reasons";
}