using SFA.DAS.Recruit.Contracts.ApiRequests;
using System.Net;
using IRecruitApiClient = SFA.DAS.Recruit.Contracts.Client.IRecruitApiClient<SFA.DAS.Recruit.Contracts.Client.RecruitApiConfiguration>;

namespace SFA.DAS.RecruitQa.Application.VacancyReviews.Queries.GetVacancyReviewsCountByAccountLegalEntity;

public class GetVacancyReviewsCountByAccountLegalEntityQueryHandler(IRecruitApiClient recruitApiClient) 
    : IRequestHandler<GetVacancyReviewsCountByAccountLegalEntityQuery, GetVacancyReviewsCountByAccountLegalEntityQueryResult>
{
    public async Task<GetVacancyReviewsCountByAccountLegalEntityQueryResult> Handle(GetVacancyReviewsCountByAccountLegalEntityQuery request, CancellationToken cancellationToken)
    {
        var apiResponse = await recruitApiClient.GetWithResponseCode<int>(
            new GetAccountsByAccountLegalEntityIdVacancyreviewsCountApiRequest(request.AccountLegalEntityId, 
                request.Status,
                request.ManualOutcome,
                request.EmployerNameOption));

        return apiResponse.StatusCode == HttpStatusCode.NotFound 
            ? new GetVacancyReviewsCountByAccountLegalEntityQueryResult { Count = 0 } 
            : new GetVacancyReviewsCountByAccountLegalEntityQueryResult { Count = apiResponse.Body };
    }
}