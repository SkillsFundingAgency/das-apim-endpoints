using SFA.DAS.EmployerFinanceJobs.InnerApi.Responses;

namespace SFA.DAS.EmployerFinanceJobs.Queries.GetEmployerFundingProjectionByAccountId;

public sealed record GetEmployerFundingProjectionByAccountIdQueryResult
{
    public long EmployerAccountId { get; init; }
    public decimal CommittedLearnerCostTotal { get; init; }
    public decimal CommittedTransferOutTotal { get; init; }
    public DateTime LastRecalculatedDate { get; init; }
    public DateTime CreatedDate { get; init; } = DateTime.UtcNow;

    public static GetEmployerFundingProjectionByAccountIdQueryResult ToResult(GetEmployerFundingProjectionByAccountIdResponse? response)
    {
        if(response is null)
            return new GetEmployerFundingProjectionByAccountIdQueryResult();

        return new GetEmployerFundingProjectionByAccountIdQueryResult
        {
            EmployerAccountId = response.EmployerAccountId,
            CommittedLearnerCostTotal = response.CommittedLearnerCostTotal,
            CommittedTransferOutTotal = response.CommittedTransferOutTotal,
            LastRecalculatedDate = response.LastRecalculatedDate,
            CreatedDate = response.CreatedDate
        };
    }
}