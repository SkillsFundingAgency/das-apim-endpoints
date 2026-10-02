
namespace SFA.DAS.EmployerFinance.InnerApi.Responses;

public sealed record MonthlyFundingBreakdown
{
    public long EmployerAccountId { get; init; }
    public int Month { get; init; }
    public int Year { get; init; }
    public decimal CommittedLearnerCost { get; init; }
    public decimal CommittedTransferOut { get; init; }
}