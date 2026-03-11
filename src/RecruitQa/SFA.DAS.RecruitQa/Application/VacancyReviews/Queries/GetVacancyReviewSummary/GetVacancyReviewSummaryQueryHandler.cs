using System.Net;
using MediatR;
using SFA.DAS.RecruitQa.InnerApi.Requests;
using SFA.DAS.RecruitQa.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.RecruitQa.Application.VacancyReviews.Queries.GetVacancyReviewSummary;

public class GetVacancyReviewSummaryQueryHandler(IRecruitApiClient<RecruitApiConfiguration> recruitApiClient)
    : IRequestHandler<GetVacancyReviewSummaryQuery, GetVacancyReviewSummaryQueryResult>
{
    public async Task<GetVacancyReviewSummaryQueryResult> Handle(GetVacancyReviewSummaryQuery request, CancellationToken cancellationToken)
    {
        var response = await recruitApiClient.GetWithResponseCode<GetVacancyReviewSummaryResponse>(
            new GetVacancyReviewSummaryRequest());

        if (response.StatusCode == HttpStatusCode.NotFound || response.Body == null)
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
