
namespace SFA.DAS.EmployerFinance.InnerApi.Responses;

public sealed record MonthlyFundingBreakdown
{
    public long EmployerAccountId { get; init; }
    public int Month { get; init; }
    public int Year { get; init; }
    public decimal ClosingBalance { get; set; } = 0m;
    public decimal CoInvestmentCosts { get; set; } = 0m;
    public required LevyInSummary LevyInSummary { get; init; }
    public required LevyOutSummary LevyOutSummary { get; init; }
}

public sealed record LevyOutSummary
{
    public decimal Total { get; init; } = 0m;
    public decimal CommittedLearnerCost { get; init; } = 0m;
    public decimal CommittedLearnerFinalPaymentCost { get; init; } = 0m;
    public decimal CommittedTransferOut { get; init; } = 0m;
    public decimal ExpiredLevy { get; init; } = 0m;
}

public sealed record LevyInSummary
{
    public decimal Total { get; init; } = 0m;
    public decimal LevyIn { get; init; } = 0m;
}