using System;

namespace SFA.DAS.EmployerFinance.InnerApi.Responses;

public sealed record GetEmployerFundingProjectionByAccountIdResponse
{
    public long EmployerAccountId { get; init; }
    public decimal CommittedLearnerCostTotal { get; init; }
    public decimal CommittedTransferOutTotal { get; init; }
    public DateTime LastRecalculatedDate { get; init; }
    public DateTime CreatedDate { get; init; } = DateTime.UtcNow;
}