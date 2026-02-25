using MediatR;
using SFA.DAS.RecruitQa.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Net;

namespace SFA.DAS.RecruitQa.Application.VacancyReviews.Queries.GetVacancyReviewsCountByAccountLegalEntity;

public class GetVacancyReviewsCountByAccountLegalEntityQueryHandler(
    IRecruitApiClient<RecruitApiConfiguration> recruitApiClient) 
    : IRequestHandler<GetVacancyReviewsCountByAccountLegalEntityQuery, GetVacancyReviewsCountByAccountLegalEntityQueryResult>
{
    public async Task<GetVacancyReviewsCountByAccountLegalEntityQueryResult> Handle(GetVacancyReviewsCountByAccountLegalEntityQuery request, CancellationToken cancellationToken)
    {
        var apiResponse = await recruitApiClient.GetWithResponseCode<int>(
            new GetVacancyReviewsCountByAccountLegalEntityRequest(request.AccountLegalEntityId, request.Status, request.ManualOutcome, request.EmployerNameOption));

        return apiResponse.StatusCode == HttpStatusCode.NotFound 
            ? new GetVacancyReviewsCountByAccountLegalEntityQueryResult { Count = 0 } 
            : new GetVacancyReviewsCountByAccountLegalEntityQueryResult { Count = apiResponse.Body };
    }
}
