using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.EmployerFinanceJobs.InnerApi.Requests;

public sealed record GetAllLearnersApiRequest(DateTime SinceTime, int BatchSize = 1000, int BatchNumber = 1) : IGetApiRequest
{
    public string GetUrl => $"api/learners?sinceTime={SinceTime:O}&batch_Size={BatchSize}&batch_Number={BatchNumber}";
}