
namespace SFA.DAS.EmployerFinance.InnerApi.Responses;

public sealed record MonthlyFundingBreakdown
{
    public int Month { get; init; }
    public int Year { get; init; }
    public decimal ClosingBalance { get; init; }
    public decimal CommittedLearnerCost { get; init; }
    public decimal CommittedLearnerFinalPaymentCost { get; init; }
    public decimal CommittedTransferOut { get; init; }
    public decimal LevyIn { get; init; }
}