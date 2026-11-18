using SFA.DAS.Recruit.Contracts.ApiRequests;
using SFA.DAS.Recruit.Contracts.ApiResponses;
using System.Net;
using IRecruitApiClient = SFA.DAS.Recruit.Contracts.Client.IRecruitApiClient<SFA.DAS.Recruit.Contracts.Client.RecruitApiConfiguration>;

namespace SFA.DAS.RecruitQa.Application.VacancyReviews.Queries.GetVacancyReviewsByUser;

public class GetVacancyReviewsByUserQueryHandler(IRecruitApiClient recruitApiClient)
    : IRequestHandler<GetVacancyReviewsByUserQuery, GetVacancyReviewsByUserQueryResult>
{
    public async Task<GetVacancyReviewsByUserQueryResult> Handle(GetVacancyReviewsByUserQuery request, CancellationToken cancellationToken)
    {
        var response = await recruitApiClient.GetWithResponseCode<List<VacancyReview>>(
            new GetUsersVacancyreviewsApiRequest(request.UserId, request.AssignationExpiry, request.Status));

        if (response.StatusCode == HttpStatusCode.NotFound)
        {
            return new GetVacancyReviewsByUserQueryResult
            {
                VacancyReviews = []
            }; 
        }

        return new GetVacancyReviewsByUserQueryResult
        {
            VacancyReviews = response.Body
        };
    }
}