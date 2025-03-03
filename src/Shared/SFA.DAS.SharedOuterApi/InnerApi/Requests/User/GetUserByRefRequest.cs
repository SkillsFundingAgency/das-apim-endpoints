using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.SharedOuterApi.InnerApi.Requests.User;

public record GetUserByRefRequest(string UserRef) : IGetApiRequest
{
    public string GetUrl => $"api/user/by-ref/{UserRef}";
}