namespace SFA.DAS.RecruitQa.Application.Dashboard.Queries.GetQaDashboard;
public record GetQaDashboardQueryResult
{
    public int TotalVacanciesForReview { get; init; }
    public int TotalVacanciesBrokenSla { get; init; }
    public int TotalVacanciesResubmitted { get; init; }
    public int TotalVacanciesSubmittedTwelveTwentyFourHours { get; init; }
    public int TotalVacanciesSubmittedLastTwelveHours { get; init; }

    public static GetQaDashboardQueryResult FromInnerApiResponse(InnerApi.Responses.GetQaDashboardApiResponse response)
    {
        return new GetQaDashboardQueryResult
        {
            TotalVacanciesForReview = response.TotalVacanciesForReview,
            TotalVacanciesBrokenSla = response.TotalVacanciesBrokenSla,
            TotalVacanciesResubmitted = response.TotalVacanciesResubmitted,
            TotalVacanciesSubmittedTwelveTwentyFourHours = response.TotalVacanciesSubmittedTwelveTwentyFourHours,
            TotalVacanciesSubmittedLastTwelveHours = response.TotalVacanciesSubmittedLastTwelveHours,
        };
    }
}
