namespace SFA.DAS.EmployerFinanceJobs.InnerApi.Responses;

public sealed record GetPeriodEndResponse
{
    public string PeriodEndId { get; set; } = string.Empty;
    public int CalendarPeriodMonth { get; set; }
    public int CalendarPeriodYear { get; set; }
    public DateTime CompletionDateTime { get; set; }
}
