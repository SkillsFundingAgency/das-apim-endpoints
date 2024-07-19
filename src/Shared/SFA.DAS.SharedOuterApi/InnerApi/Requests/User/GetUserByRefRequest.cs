using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.SharedOuterApi.InnerApi.Requests.User;

public record GetUserByRefRequest(string userRef) : IGetApiRequest
{
    public string GetUrl => $"api/user/by-ref/{userRef}";
}