namespace SFA.DAS.EmployerFinance.InnerApi.Responses.Finance;

public sealed record CalendarPeriodAmount
{
    public int Year { get; init; }
    public int Month { get; init; }
    public decimal Amount { get; init; }
}
