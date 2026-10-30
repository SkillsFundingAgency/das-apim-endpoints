using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.EmployerFinanceJobs.InnerApi.Requests;

public sealed record GetProviderPaymentsPriorityRequest(long AccountId) : IGetApiRequest
{
    public string GetUrl => $"api/accounts/{AccountId}/provider-payments-priority";
}