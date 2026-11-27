using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.EmployerFinance.InnerApi.Requests.Finance;

public sealed record GetImportJobStateRequest(long AccountId) : IGetApiRequest
{
    public string GetUrl => $"api/jobs/{AccountId}/ImportLearnersCost";
}
