using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.ToolsSupport.InnerApi.Requests;

public class GetUserProfileRequest(string userRef) : IGetApiRequest
{
    public string GetUrl => $"api/users/{userRef}";
}