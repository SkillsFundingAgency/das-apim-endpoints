namespace SFA.DAS.EmployerFinance.Application.Queries.GetLevySummaryByAccountId;

public sealed record GetLevySummaryByAccountIdQueryResult
{
    public decimal CurrentLevyFunds { get; init; }
    public decimal TotalLevyDeclaredLast12Months { get; init; }
    public decimal TotalLevySpentLast12Months { get; init; }
    public decimal TotalLevyExpiredLast12Months { get; init; }
}