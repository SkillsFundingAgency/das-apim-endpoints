using SFA.DAS.Apim.Shared.Interfaces;
using SFA.DAS.EmployerFinanceJobs.Domain.Enums;

namespace SFA.DAS.EmployerFinanceJobs.InnerApi.Requests;

public sealed record GetOrCreateJobApiRequest(JobName JobName) : IPostApiRequest
{
    public string PostUrl => $"api/jobs/{JobName}";
    public object Data { get; set; } = null;
}