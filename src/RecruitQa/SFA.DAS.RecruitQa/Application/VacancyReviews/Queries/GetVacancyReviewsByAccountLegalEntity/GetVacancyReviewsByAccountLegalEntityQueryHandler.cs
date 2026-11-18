using SFA.DAS.Recruit.Contracts.ApiRequests;
using SFA.DAS.Recruit.Contracts.ApiResponses;
using System.Net;
using IRecruitApiClient = SFA.DAS.Recruit.Contracts.Client.IRecruitApiClient<SFA.DAS.Recruit.Contracts.Client.RecruitApiConfiguration>;

namespace SFA.DAS.RecruitQa.Application.VacancyReviews.Queries.GetVacancyReviewsByAccountLegalEntity;

public class GetVacancyReviewsByAccountLegalEntityQueryHandler(IRecruitApiClient recruitApiClient)
    : IRequestHandler<GetVacancyReviewsByAccountLegalEntityQuery, GetVacancyReviewsByAccountLegalEntityQueryResult>
{
    public async Task<GetVacancyReviewsByAccountLegalEntityQueryResult> Handle(GetVacancyReviewsByAccountLegalEntityQuery request, CancellationToken cancellationToken)
    {
        var response = await recruitApiClient.GetWithResponseCode<List<VacancyReview>>(
            new GetAccountsByAccountLegalEntityIdVacancyreviewsApiRequest(request.AccountLegalEntityId));

        if (response.StatusCode == HttpStatusCode.NotFound)
        {
            return new GetVacancyReviewsByAccountLegalEntityQueryResult
            {
                VacancyReviews = []
            };
        }

        return new GetVacancyReviewsByAccountLegalEntityQueryResult
        {
            VacancyReviews = response.Body
        };
    }
}