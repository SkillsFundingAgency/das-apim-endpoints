using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.ToolsSupport.InnerApi.Requests;
public class GetPrivilegedAccessTokenRequest : IGetApiRequest
{
    public string GetUrl => "api/PrivilegedAccess";
}