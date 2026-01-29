using System.Net;
using MediatR;
using SFA.DAS.RecruitQa.InnerApi.Requests;
using SFA.DAS.RecruitQa.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.RecruitQa.Application.VacancyReviews.Queries.GetVacancyReviewsByFilter;

public class GetVacancyReviewsByFilterQueryHandler(IRecruitApiClient<RecruitApiConfiguration> recruitApiClient)
    : IRequestHandler<GetVacancyReviewsByFilterQuery, GetVacancyReviewsByFilterQueryResult>
{
    public async Task<GetVacancyReviewsByFilterQueryResult> Handle(GetVacancyReviewsByFilterQuery request, CancellationToken cancellationToken)
    {
        var response = await recruitApiClient.GetWithResponseCode<List<GetVacancyReviewResponse>>(
            new GetVacancyReviewsByFilterRequest(request.Status, request.ExpiredAssignationDateTime));

        if (response.StatusCode == HttpStatusCode.NotFound || response.Body == null)
        {
            return new GetVacancyReviewsByFilterQueryResult
            {
                VacancyReviews = new List<GetVacancyReviewResponse>()
            };
        }

        return new GetVacancyReviewsByFilterQueryResult
        {
            VacancyReviews = response.Body
        };
    }
}
