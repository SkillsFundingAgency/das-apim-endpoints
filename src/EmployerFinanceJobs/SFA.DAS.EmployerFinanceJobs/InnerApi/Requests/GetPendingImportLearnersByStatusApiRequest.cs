using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.EmployerFinanceJobs.InnerApi.Requests;

public sealed record GetPendingImportLearnersByStatusApiRequest(string Status) : IGetApiRequest
{
    public string GetUrl => $"api/employer/learners/by/status?importStatus={Status}";
}