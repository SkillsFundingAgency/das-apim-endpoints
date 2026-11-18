using SFA.DAS.Recruit.Contracts.ApiRequests;
using SFA.DAS.Recruit.Contracts.ApiResponses;
using System.Net;
using IRecruitApiClient = SFA.DAS.Recruit.Contracts.Client.IRecruitApiClient<SFA.DAS.Recruit.Contracts.Client.RecruitApiConfiguration>;

namespace SFA.DAS.RecruitQa.Application.VacancyReviews.Queries.GetVacancyReview;

public class GetVacancyReviewQueryHandler(IRecruitApiClient recruitApiClient) : IRequestHandler<GetVacancyReviewQuery, GetVacancyReviewQueryResult>
{
    public async Task<GetVacancyReviewQueryResult> Handle(GetVacancyReviewQuery request, CancellationToken cancellationToken)
    {
        var result = await recruitApiClient.GetWithResponseCode<VacancyReview>(new GetVacancyreviewsByIdApiRequest(request.Id));

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