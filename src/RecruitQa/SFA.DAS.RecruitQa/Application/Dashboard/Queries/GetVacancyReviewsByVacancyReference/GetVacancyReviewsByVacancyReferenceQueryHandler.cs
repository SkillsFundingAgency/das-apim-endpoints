using System.Net;
using MediatR;
using SFA.DAS.RecruitQa.InnerApi.Requests;
using SFA.DAS.RecruitQa.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.RecruitQa.Application.Dashboard.Queries.GetVacancyReviewsByVacancyReference;

public class GetVacancyReviewsByVacancyReferenceQueryHandler(IRecruitApiClient<RecruitApiConfiguration> recruitApiClient)
    : IRequestHandler<GetVacancyReviewsByVacancyReferenceQuery, GetVacancyReviewsByVacancyReferenceQueryResult>
{
    public async Task<GetVacancyReviewsByVacancyReferenceQueryResult> Handle(GetVacancyReviewsByVacancyReferenceQuery request, CancellationToken cancellationToken)
    {
        var response = await recruitApiClient.GetWithResponseCode<List<GetVacancyReviewResponse>>(
            new GetVacancyReviewsByVacancyReferenceRequest(request.VacancyReference, request.Status, request.IncludeNoStatus, request.ManualOutcome));

        if (response.StatusCode == HttpStatusCode.NotFound || response.Body == null)
        {
            return new GetVacancyReviewsByVacancyReferenceQueryResult
            {
                VacancyReviews = new List<GetVacancyReviewResponse>()
            };
        }

        return new GetVacancyReviewsByVacancyReferenceQueryResult
        {
            VacancyReviews = response.Body
        };
    }
}
