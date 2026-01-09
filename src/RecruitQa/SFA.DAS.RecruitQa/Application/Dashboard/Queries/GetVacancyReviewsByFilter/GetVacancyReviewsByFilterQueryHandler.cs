using System.Net;
using MediatR;
using SFA.DAS.RecruitQa.InnerApi.Requests;
using SFA.DAS.RecruitQa.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.RecruitQa.Application.Dashboard.Queries.GetVacancyReviewsByFilter;

public class GetVacancyReviewsByFilterQueryHandler(IRecruitApiClient<RecruitApiConfiguration> recruitApiClient)
    : IRequestHandler<GetVacancyReviewsByFilterQuery, GetVacancyReviewsByFilterQueryResult>
{
    public async Task<GetVacancyReviewsByFilterQueryResult> Handle(GetVacancyReviewsByFilterQuery request, CancellationToken cancellationToken)
    {
        var statuses = request.Status == null
            ? null
            : new List<string> { request.Status };

        var response = await recruitApiClient.GetWithResponseCode<List<GetVacancyReviewResponse>>(
            new GetVacancyReviewsByFilterRequest(statuses, request.ExpiredAssignationDateTime));

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
