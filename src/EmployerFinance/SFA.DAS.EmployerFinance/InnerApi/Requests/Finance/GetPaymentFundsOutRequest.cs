using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.EmployerFinance.InnerApi.Requests.Finance;

public sealed record GetPaymentFundsOutRequest(long AccountId) : IGetApiRequest
{
    public string GetUrl => $"api/payments/account/{AccountId}/funds-out";
}
