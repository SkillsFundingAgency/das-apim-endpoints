namespace SFA.DAS.EmployerFinance.Application.Queries.GetLevySummaryByHashedAccountId;

public sealed record GetLevySummaryByHashedAccountIdQueryResult
{
    public decimal CurrentLevyFunds { get; init; }
}