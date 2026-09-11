namespace SFA.DAS.EmployerFinance.InnerApi.Responses;

public sealed record GetCommittedCostsByAccountIdResponse
{
    public decimal TotalCommittedLearnerCosts { get; init; }
    public decimal TotalCommittedTransfersCosts { get; init; }
}