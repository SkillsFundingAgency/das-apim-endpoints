using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.ToolsSupport.InnerApi.Requests;

public class GetUserByIdRequest(Guid userId) : IGetApiRequest
{
    public string GetUrl => $"api/users/{userId}";
}
