using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Recruit.InnerApi.Requests;
using SFA.DAS.Recruit.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Recruit.Application.ApplicationReview.Queries.GetApplicationReviewsByVacancyReferenceAndTempStatus;
public class GetApplicationReviewsByVacancyReferenceAndTempStatusQueryHandler(
    IRecruitApiClient<RecruitApiConfiguration> recruitApiClient,
    ICandidateApiClient<CandidateApiConfiguration> candidateApiClient) 
    : IRequestHandler<GetApplicationReviewsByVacancyReferenceAndTempStatusQuery, GetApplicationReviewsByVacancyReferenceAndTempStatusQueryResult>
{
    public async Task<GetApplicationReviewsByVacancyReferenceAndTempStatusQueryResult> Handle(GetApplicationReviewsByVacancyReferenceAndTempStatusQuery request, CancellationToken cancellationToken)
    {
        var recruitApiTask = recruitApiClient.Get<List<Domain.ApplicationReview>>(
            new GetApplicationReviewsByVacancyReferenceAndTempStatusApiRequest(request.VacancyReference, request.Status));

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

            return new GetApplicationReviewsByVacancyReferenceAndTempStatusQueryResult(recruitResponse);
        }

        return new GetApplicationReviewsByVacancyReferenceAndTempStatusQueryResult([]);
    }
}
