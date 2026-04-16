using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.SharedOuterApi.InnerApi.Requests.Roatp;
public class GetRemovedReasonsRequest : IGetApiRequest
{
    public string GetUrl => "removed-reasons";
}