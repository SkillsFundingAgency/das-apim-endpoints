using MediatR;
using SFA.DAS.Recruit.InnerApi.Requests;
using SFA.DAS.Recruit.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.Recruit.Application.ApplicationReview.Queries.GetApplicationReviewByVacancyReferenceAndCandidateId;
public class GetApplicationReviewByVacancyReferenceAndCandidateIdQueryHandler(
    IRecruitApiClient<RecruitApiConfiguration> recruitApiClient,
    ICandidateApiClient<CandidateApiConfiguration> candidateApiClient) :
    IRequestHandler<GetApplicationReviewByVacancyReferenceAndCandidateIdQuery, GetApplicationReviewByVacancyReferenceAndCandidateIdQueryResult>
{
    public async Task<GetApplicationReviewByVacancyReferenceAndCandidateIdQueryResult> Handle(GetApplicationReviewByVacancyReferenceAndCandidateIdQuery request, CancellationToken cancellationToken)
    {
        var recruitApiTask = recruitApiClient.Get<Domain.ApplicationReview>(
            new GetApplicationReviewByVacancyReferenceAndCandidateIdApiRequest(request.VacancyReference, request.CandidateId));

        var candidateApiTask = candidateApiClient.Get<GetApplicationsByVacancyReferenceApiResponse>(
            new GetApplicationsByVacancyReferenceApiRequest(request.VacancyReference));

        await Task.WhenAll(recruitApiTask, candidateApiTask);

        var recruitResponse = recruitApiTask.Result;
        var candidateResponse = candidateApiTask.Result;

        if (recruitResponse != null)
        {
                var application =
                    candidateResponse?.Applications?.Find(a => a.Id == recruitResponse.ApplicationId);
                recruitResponse.Application = application;

            return new GetApplicationReviewByVacancyReferenceAndCandidateIdQueryResult
            {
                ApplicationReview = recruitResponse
            };
        }

        return new GetApplicationReviewByVacancyReferenceAndCandidateIdQueryResult();
    }
}