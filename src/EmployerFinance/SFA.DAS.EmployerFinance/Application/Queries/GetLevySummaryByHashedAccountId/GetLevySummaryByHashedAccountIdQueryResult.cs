namespace SFA.DAS.EmployerFinance.Application.Queries.GetLevySummaryByHashedAccountId;

public sealed record GetLevySummaryByHashedAccountIdQueryResult
{
    public decimal CurrentLevyFunds { get; init; }
    public decimal TotalLevyDeclaredLast12Months { get; init; }
    public decimal TotalLevySpentLast12Months { get; init; }
}