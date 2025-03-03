using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Approvals.InnerApi.Requests;

public class GetLegalEntitiesForAccountRequest(long accountId) : IGetApiRequest
{
    private readonly long _accountId = accountId;

    public string GetUrl => $"api/accounts/{_accountId}/legalentities?includeDetails=true";
}