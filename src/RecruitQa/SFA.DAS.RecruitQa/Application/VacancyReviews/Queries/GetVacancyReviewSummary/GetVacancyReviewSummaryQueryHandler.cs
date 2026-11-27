using SFA.DAS.Recruit.Contracts.ApiRequests;
using SFA.DAS.Recruit.Contracts.ApiResponses;
using System.Net;
using IRecruitApiClient = SFA.DAS.Recruit.Contracts.Client.IRecruitApiClient<SFA.DAS.Recruit.Contracts.Client.RecruitApiConfiguration>;

namespace SFA.DAS.RecruitQa.Application.VacancyReviews.Queries.GetVacancyReviewSummary;

public class GetVacancyReviewSummaryQueryHandler(IRecruitApiClient recruitApiClient)
    : IRequestHandler<GetVacancyReviewSummaryQuery, GetVacancyReviewSummaryQueryResult>
{
    public async Task<GetVacancyReviewSummaryQueryResult> Handle(GetVacancyReviewSummaryQuery request, CancellationToken cancellationToken)
    {
        var response = await recruitApiClient.GetWithResponseCode<QaDashboard>(
            new GetVacancyreviewsQaDashboardApiRequest());

        if (response.StatusCode == HttpStatusCode.NotFound)
        {
            return new GetVacancyReviewSummaryQueryResult
            {
                TotalVacanciesForReview = 0,
                TotalVacanciesResubmitted = 0,
                TotalVacanciesBrokenSla = 0,
                TotalVacanciesSubmittedTwelveTwentyFourHours = 0
            };
        }

        return new GetVacancyReviewSummaryQueryResult
        {
            TotalVacanciesForReview = response.Body.TotalVacanciesForReview,
            TotalVacanciesResubmitted = response.Body.TotalVacanciesResubmitted,
            TotalVacanciesBrokenSla = response.Body.TotalVacanciesBrokenSla,
            TotalVacanciesSubmittedTwelveTwentyFourHours = response.Body.TotalVacanciesSubmittedTwelveTwentyFourHours
        };
    }
}