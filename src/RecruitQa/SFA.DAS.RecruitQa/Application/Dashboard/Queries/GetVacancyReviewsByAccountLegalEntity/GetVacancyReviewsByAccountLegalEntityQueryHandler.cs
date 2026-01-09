using System.Net;
using MediatR;
using SFA.DAS.RecruitQa.InnerApi.Requests;
using SFA.DAS.RecruitQa.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.RecruitQa.Application.Dashboard.Queries.GetVacancyReviewsByAccountLegalEntity;

public class GetVacancyReviewsByAccountLegalEntityQueryHandler(IRecruitApiClient<RecruitApiConfiguration> recruitApiClient)
    : IRequestHandler<GetVacancyReviewsByAccountLegalEntityQuery, GetVacancyReviewsByAccountLegalEntityQueryResult>
{
    public async Task<GetVacancyReviewsByAccountLegalEntityQueryResult> Handle(GetVacancyReviewsByAccountLegalEntityQuery request, CancellationToken cancellationToken)
    {
        var response = await recruitApiClient.GetWithResponseCode<List<GetVacancyReviewResponse>>(
            new GetVacancyReviewsByAccountLegalEntityRequest(request.AccountLegalEntityId));

        if (response.StatusCode == HttpStatusCode.NotFound || response.Body == null)
        {
            return new GetVacancyReviewsByAccountLegalEntityQueryResult
            {
                VacancyReviews = new List<GetVacancyReviewResponse>()
            };
        }

        return new GetVacancyReviewsByAccountLegalEntityQueryResult
        {
            VacancyReviews = response.Body
        };
    }
}