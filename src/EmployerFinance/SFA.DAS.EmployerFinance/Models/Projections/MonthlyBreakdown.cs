namespace SFA.DAS.EmployerFinance.Models.Projections;

public sealed record MonthlyBreakdown
{
    public int CalendarPeriodMonth { get; init; } = 0;
    public int CalendarPeriodYear { get; init; } = 0;
    public string CalendarMonthName { get; init; }
    public decimal LevyIn { get; init; } = 0;
    public decimal ExpiredLevy { get; init; } = 0;
}