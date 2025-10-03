using MediatR;
using SFA.DAS.Recruit.InnerApi.Requests;
using SFA.DAS.Recruit.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.Recruit.Application.ApplicationReview.Queries.GetApplicationReviewsByVacancyReferenceAndStatus;
public class GetApplicationReviewsByVacancyReferenceAndStatusQueryHandler(
    IRecruitApiClient<RecruitApiConfiguration> recruitApiClient,
    ICandidateApiClient<CandidateApiConfiguration> candidateApiClient) : IRequestHandler<GetApplicationReviewsByVacancyReferenceAndStatusQuery, GetApplicationReviewsByVacancyReferenceAndStatusQueryResult>
{
    public async Task<GetApplicationReviewsByVacancyReferenceAndStatusQueryResult> Handle(GetApplicationReviewsByVacancyReferenceAndStatusQuery request, CancellationToken cancellationToken)
    {
        var recruitApiTask = recruitApiClient.Get<List<Domain.ApplicationReview>>(
            new GetApplicationReviewsByVacancyReferenceAndStatusApiRequest(request.VacancyReference, request.Status, request.IncludeTemporaryStatus));

        var candidateApiTask = candidateApiClient.Get<GetApplicationsByVacancyReferenceApiResponse>(
            new GetApplicationsByVacancyReferenceApiRequest(request.VacancyReference));

        await Task.WhenAll(recruitApiTask, candidateApiTask);

        var recruitResponse = recruitApiTask.Result;
        var candidateResponse = candidateApiTask.Result;

        if (recruitResponse != null)
        {
            foreach (var applicationReview in recruitResponse)
            {
                var application =
                    candidateResponse?.Applications?.Find(a => a.Id == applicationReview.ApplicationId);
                applicationReview.Application = application;
            }

            return new GetApplicationReviewsByVacancyReferenceAndStatusQueryResult(recruitResponse);
        }

        return new GetApplicationReviewsByVacancyReferenceAndStatusQueryResult([]);
    }
}
