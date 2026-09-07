using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.EmployerFinance.InnerApi.Requests;

public record GetLevySummaryByHashedAccountIdRequest(string HashedAccountId) : IGetApiRequest
{
    public string GetUrl => $"api/accounts/{HashedAccountId}/levy/summary";
}