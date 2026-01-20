using System.Net;
using MediatR;
using SFA.DAS.RecruitQa.InnerApi.Requests;
using SFA.DAS.RecruitQa.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.RecruitQa.Application.Dashboard.Queries.GetVacancyReviewsByUser;

public class GetVacancyReviewsByUserQueryHandler(IRecruitApiClient<RecruitApiConfiguration> recruitApiClient)
    : IRequestHandler<GetVacancyReviewsByUserQuery, GetVacancyReviewsByUserQueryResult>
{
    public async Task<GetVacancyReviewsByUserQueryResult> Handle(GetVacancyReviewsByUserQuery request, CancellationToken cancellationToken)
    {
        var response = await recruitApiClient.GetWithResponseCode<List<GetVacancyReviewResponse>>(
            new GetVacancyReviewsByUserRequest(request.UserId, request.AssignationExpiry, request.Status));

        if (response.StatusCode == HttpStatusCode.NotFound || response.Body == null)
        {
            return new GetVacancyReviewsByUserQueryResult
            {
                VacancyReviews = new List<GetVacancyReviewResponse>()
            };
        }

        return new GetVacancyReviewsByUserQueryResult
        {
            VacancyReviews = response.Body
        };
    }
}
