using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.EmployerFinanceJobs.InnerApi.Requests;

public sealed record GetApprenticeshipPaymentByAccountIdRequest(long AccountId, long ApprenticeshipId) : IGetApiRequest
{
    public string GetUrl => $"api/payments/account/{AccountId}/apprenticeship/{ApprenticeshipId}";
}