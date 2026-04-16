namespace SFA.DAS.RecruitQa.InnerApi.Responses;

public class GetVacancyReviewSummaryResponse
{
    public int TotalVacanciesForReview { get; set; }
    public int TotalVacanciesResubmitted { get; set; }
    public int TotalVacanciesBrokenSla { get; set; }
    public int TotalVacanciesSubmittedTwelveTwentyFourHours { get; set; }
}
