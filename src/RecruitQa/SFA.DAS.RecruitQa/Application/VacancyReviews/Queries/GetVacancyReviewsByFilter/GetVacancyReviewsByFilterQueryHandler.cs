using System.Net;
using SFA.DAS.Recruit.Contracts.ApiRequests;
using SFA.DAS.Recruit.Contracts.ApiResponses;
using IRecruitApiClient = SFA.DAS.Recruit.Contracts.Client.IRecruitApiClient<SFA.DAS.Recruit.Contracts.Client.RecruitApiConfiguration>;

namespace SFA.DAS.RecruitQa.Application.VacancyReviews.Queries.GetVacancyReviewsByFilter;

public class GetVacancyReviewsByFilterQueryHandler(IRecruitApiClient recruitApiClient)
    : IRequestHandler<GetVacancyReviewsByFilterQuery, GetVacancyReviewsByFilterQueryResult>
{
    public async Task<GetVacancyReviewsByFilterQueryResult> Handle(GetVacancyReviewsByFilterQuery request, CancellationToken cancellationToken)
    {
        var response = await recruitApiClient.GetWithResponseCode<List<VacancyReview>>(
            new GetVacancyreviewsApiRequest(request.Status, request.ExpiredAssignationDateTime));

        if (response.StatusCode == HttpStatusCode.NotFound)
        {
            return new GetVacancyReviewsByFilterQueryResult
            {
                VacancyReviews = []
            };
        }

        return new GetVacancyReviewsByFilterQueryResult
        {
            VacancyReviews = response.Body
        };
    }
}