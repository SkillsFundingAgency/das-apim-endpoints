namespace SFA.DAS.EmployerFinance.Models.Projections;

public sealed record LevyProjectionsSummary
{
    public decimal CurrentLevyFunds { get; init; } = 0;
    public decimal TotalLevyDeclaredLast12Months { get; init; } = 0;
    public decimal TotalLevySpentLast12Months { get; init; } = 0;
    public decimal TotalLevyExpiredLast12Months { get; init; } = 0;
    public decimal TotalCommittedLearnerCosts { get; init; } = 0;
    public decimal TotalCommittedTransfersCosts { get; init; } = 0;
}