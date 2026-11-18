using SFA.DAS.Recruit.Contracts.ApiRequests;
using SFA.DAS.Recruit.Contracts.ApiResponses;
using System.Net;
using IRecruitApiClient = SFA.DAS.Recruit.Contracts.Client.IRecruitApiClient<SFA.DAS.Recruit.Contracts.Client.RecruitApiConfiguration>;

namespace SFA.DAS.RecruitQa.Application.VacancyReviews.Queries.GetVacancyReviewsByVacancyReference;

public class GetVacancyReviewsByVacancyReferenceQueryHandler(IRecruitApiClient recruitApiClient)
    : IRequestHandler<GetVacancyReviewsByVacancyReferenceQuery, GetVacancyReviewsByVacancyReferenceQueryResult>
{
    public async Task<GetVacancyReviewsByVacancyReferenceQueryResult> Handle(GetVacancyReviewsByVacancyReferenceQuery request, CancellationToken cancellationToken)
    {
        var response = await recruitApiClient.GetWithResponseCode<List<VacancyReview>>(
            new GetVacanciesByVacancyReferenceReviewsApiRequest(request.VacancyReference.ToString(), request.Status, request.ManualOutcome, request.IncludeNoStatus));

        if (response.StatusCode == HttpStatusCode.NotFound)
        {
            return new GetVacancyReviewsByVacancyReferenceQueryResult
            {
                VacancyReviews = []
            };
        }

        return new GetVacancyReviewsByVacancyReferenceQueryResult
        {
            VacancyReviews = response.Body
        };
    }
}