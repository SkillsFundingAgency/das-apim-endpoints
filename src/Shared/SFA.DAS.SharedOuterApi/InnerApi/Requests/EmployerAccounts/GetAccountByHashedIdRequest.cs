using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.SharedOuterApi.InnerApi.Requests.EmployerAccounts;

public class GetAccountByHashedIdRequest(string hashedAccountId) : IGetApiRequest
{
    public string GetUrl => $"api/accounts/{hashedAccountId}";
}