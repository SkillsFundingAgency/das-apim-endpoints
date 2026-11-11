using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.EmployerFinanceJobs.InnerApi.Requests;

public sealed record GetApprenticeshipPaymentSummariesRequest(long AccountId, DateOnly AwaitingPaymentFor) : IGetApiRequest
{
    public string GetUrl => $"api/employer/{AccountId}/apprenticeship-payment-summary?awaitingPaymentFor={AwaitingPaymentFor:yyyy-MM-dd}";
}
