using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.EmployerFinanceJobs.InnerApi.Requests;

public sealed record RecalculateFundingProjectionApiRequest(DateTime CutOffDateTime) : IPostApiRequest
{
    public string PostUrl => "api/employer/funding-projection/re-calculate";
    public object Data { set; get; } = CutOffDateTime.ToString("O");
}