using System.Net;
using MediatR;
using SFA.DAS.RecruitQa.InnerApi.Requests;
using SFA.DAS.RecruitQa.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.RecruitQa.Application.VacancyReviews.Queries.GetVacancyReview;

public class GetVacancyReviewQueryHandler(IRecruitApiClient<RecruitApiConfiguration> recruitApiClient) : IRequestHandler<GetVacancyReviewQuery, GetVacancyReviewQueryResult>
{
    public async Task<GetVacancyReviewQueryResult> Handle(GetVacancyReviewQuery request, CancellationToken cancellationToken)
    {
        var result =
            await recruitApiClient.GetWithResponseCode<GetVacancyReviewResponse>(
                new GetVacancyReviewByIdRequest(request.Id));

        if (result.StatusCode == HttpStatusCode.NotFound)
        {
            return new GetVacancyReviewQueryResult
            {
                VacancyReview = null
            };
        }

        return new GetVacancyReviewQueryResult
        {
            VacancyReview = result.Body
        };
    }
}