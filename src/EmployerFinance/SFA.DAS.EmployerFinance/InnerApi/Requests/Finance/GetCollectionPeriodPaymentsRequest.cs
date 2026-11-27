using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.EmployerFinance.InnerApi.Requests.Finance;

public sealed record GetCollectionPeriodPaymentsRequest(long AccountId, int Year, int Month) : IGetApiRequest
{
    public string GetUrl => $"api/payments/account/{AccountId}/collection-period/{Year}/{Month}";
}
