using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.SharedOuterApi.InnerApi.Requests.EmployerAccounts;

public class GetAccountByIdRequest(long accountId) : IGetApiRequest
{
    public long AccountId { get; } = accountId;

    public string GetUrl => $"api/accounts/{AccountId}";
}