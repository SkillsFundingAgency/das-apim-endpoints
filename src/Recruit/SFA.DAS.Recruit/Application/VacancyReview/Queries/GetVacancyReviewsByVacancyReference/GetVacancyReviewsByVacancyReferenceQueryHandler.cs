using MediatR;
using SFA.DAS.Recruit.InnerApi.Recruit.Requests.VacancyReviews;
using SFA.DAS.Recruit.InnerApi.Recruit.Responses;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.Interfaces;

namespace SFA.DAS.Recruit.Application.VacancyReview.Queries.GetVacancyReviewsByVacancyReference;

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