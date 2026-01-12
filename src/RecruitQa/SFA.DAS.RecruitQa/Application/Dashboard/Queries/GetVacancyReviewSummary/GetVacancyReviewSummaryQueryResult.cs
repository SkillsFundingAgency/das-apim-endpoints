namespace SFA.DAS.RecruitQa.Application.Dashboard.Queries.GetVacancyReviewSummary;

public class GetVacancyReviewSummaryQueryResult
{
    public int TotalVacanciesForReview { get; set; }
    public int TotalVacanciesResubmitted { get; set; }
    public int TotalVacanciesBrokenSla { get; set; }
    public int TotalVacanciesSubmittedTwelveTwentyFourHours { get; set; }
}
