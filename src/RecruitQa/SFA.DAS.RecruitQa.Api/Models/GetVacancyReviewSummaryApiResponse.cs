namespace SFA.DAS.RecruitQa.Api.Models;

public class GetVacancyReviewSummaryApiResponse
{
    public int TotalVacanciesForReview { get; set; }
    public int TotalVacanciesResubmitted { get; set; }
    public int TotalVacanciesBrokenSla { get; set; }
    public int TotalVacanciesSubmittedTwelveTwentyFourHours { get; set; }
}
