using SFA.DAS.Recruit.Contracts.ApiRequests;
using System.Net;
using IRecruitApiClient = SFA.DAS.Recruit.Contracts.Client.IRecruitApiClient<SFA.DAS.Recruit.Contracts.Client.RecruitApiConfiguration>;

namespace SFA.DAS.RecruitQa.Application.VacancyReviews.Queries.GetVacancyReviewsCountByUser;

public class GetVacancyReviewsCountByUserQueryHandler(IRecruitApiClient recruitApiClient)
    : IRequestHandler<GetVacancyReviewsCountByUserQuery, GetVacancyReviewsCountByUserQueryResult>
{
    public async Task<GetVacancyReviewsCountByUserQueryResult> Handle(GetVacancyReviewsCountByUserQuery request, CancellationToken cancellationToken)
    {
        var response = await recruitApiClient.GetWithResponseCode<int>(
            new GetUsersVacancyreviewsCountApiRequest(request.UserEmail, request.ApprovedFirstTime, request.AssignationExpiry));

        return response.StatusCode == HttpStatusCode.NotFound 
            ? new GetVacancyReviewsCountByUserQueryResult { Count = 0 } 
            : new GetVacancyReviewsCountByUserQueryResult { Count = response.Body };
    }
}