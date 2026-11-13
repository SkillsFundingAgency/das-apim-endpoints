using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.EmployerFinance.InnerApi.Requests.EmployerAccounts;

public class GetAccountPayeSchemesRequest(long accountId) : IGetApiRequest
{
    public string GetUrl => $"api/accounts/{accountId}/payeschemes";
}