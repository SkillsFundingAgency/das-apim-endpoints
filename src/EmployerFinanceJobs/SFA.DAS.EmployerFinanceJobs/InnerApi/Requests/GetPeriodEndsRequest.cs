using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.EmployerFinanceJobs.InnerApi.Requests;

public sealed record GetPeriodEndsRequest : IGetApiRequest
{
    public string GetUrl => "api/period-ends";
}
