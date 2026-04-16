namespace SFA.DAS.RecruitJobs.Domain.Models;

public record VacancyAnalytics
{
    public required DateTime AnalyticsDate { get; set; }
    public int ViewsCount { get; set; } = 0;
    public int SearchResultsCount { get; set; } = 0;
    public int ApplicationStartedCount { get; set; } = 0;
    public int ApplicationSubmittedCount { get; set; } = 0;
}