using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.ToolsSupport.InnerApi.Requests;

public class GetUserProfileRequest(string userRef) : IGetApiRequest
{
    public string GetUrl => $"api/users/{userRef}";
}