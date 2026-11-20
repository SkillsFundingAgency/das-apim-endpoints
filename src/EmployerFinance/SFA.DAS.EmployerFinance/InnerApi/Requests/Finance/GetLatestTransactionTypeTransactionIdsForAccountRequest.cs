using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.EmployerFinance.InnerApi.Requests.Finance;

public sealed record GetLatestTransactionTypeTransactionIdsForAccountRequest(long AccountId) : IGetApiRequest
{
    public string GetUrl => $"api/transaction-lines/{AccountId}/latest-transaction-type-ids";
}
