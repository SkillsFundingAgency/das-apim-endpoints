using System.Net;
using MediatR;
using SFA.DAS.RecruitQa.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.RecruitQa.Application.Dashboard.Queries.GetVacancyReviewsCountByUser;

public class GetVacancyReviewsCountByUserQueryHandler(IRecruitApiClient<RecruitApiConfiguration> recruitApiClient)
    : IRequestHandler<GetVacancyReviewsCountByUserQuery, GetVacancyReviewsCountByUserQueryResult>
{
    public async Task<GetVacancyReviewsCountByUserQueryResult> Handle(GetVacancyReviewsCountByUserQuery request, CancellationToken cancellationToken)
    {
        var response = await recruitApiClient.GetWithResponseCode<int>(
            new GetVacancyReviewsCountByUserRequest(request.UserId, request.ApprovedFirstTime));

        if (response.StatusCode == HttpStatusCode.NotFound)
        {
            return new GetVacancyReviewsCountByUserQueryResult { Count = 0 };
        }

        return new GetVacancyReviewsCountByUserQueryResult { Count = response.Body };
    }
}
