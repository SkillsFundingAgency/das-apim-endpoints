using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.SharedOuterApi.InnerApi.Requests;

public record GetAccountUsersRequest(long AccountId) : IGetApiRequest
{
    public string GetUrl => $"api/accounts/{AccountId}/users";
}