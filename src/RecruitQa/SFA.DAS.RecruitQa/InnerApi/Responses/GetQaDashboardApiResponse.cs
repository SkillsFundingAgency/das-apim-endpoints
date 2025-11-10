namespace SFA.DAS.RecruitQa.InnerApi.Responses;
public record GetQaDashboardApiResponse
{
    public int TotalVacanciesForReview { get; set; } = 0;
    public int TotalVacanciesBrokenSla { get; set; } = 0;
    public int TotalVacanciesResubmitted { get; set; } = 0;
    public int TotalVacanciesSubmittedTwelveTwentyFourHours { get; set; } = 0;
    public int TotalVacanciesSubmittedLastTwelveHours { get; set; } = 0;
}