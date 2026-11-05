using System.Collections.Generic;

namespace SFA.DAS.EmployerFinance.InnerApi.Responses.Finance;

public sealed record CollectionPeriodPayments
{
    public bool PeriodEndCompleted { get; init; }
    public List<CollectionPeriodPaymentTotal> Payments { get; init; } = [];
}

public sealed record CollectionPeriodPaymentTotal
{
    public int FundingSource { get; init; }
    public int TransactionType { get; init; }
    public decimal Amount { get; init; }
}
