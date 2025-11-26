using MediatR;
using SFA.DAS.Recruit.InnerApi.Recruit.Responses;
using SFA.DAS.Recruit.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.Recruit.Application.ApplicationReview.Queries.GetApplicationReviewsByVacancyReference
{
    public class GetApplicationReviewsByVacancyReferenceQueryHandler(
        IRecruitApiClient<RecruitApiConfiguration> recruitApiClient,
        ICandidateApiClient<CandidateApiConfiguration> candidateApiClient) : IRequestHandler<GetApplicationReviewsByVacancyReferenceQuery, GetApplicationReviewsByVacancyReferenceQueryResult>
    {
        public async Task<GetApplicationReviewsByVacancyReferenceQueryResult> Handle(GetApplicationReviewsByVacancyReferenceQuery request, CancellationToken cancellationToken)
        {
            var recruitApiTask = recruitApiClient.Get<List<Domain.ApplicationReview>>(
                new GetApplicationReviewsByVacancyReferenceApiRequest(request.VacancyReference));

            var candidateApiTask = candidateApiClient.Get<GetApplicationsByVacancyReferenceApiResponse> (
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

                return new GetApplicationReviewsByVacancyReferenceQueryResult(recruitResponse);
            }

            return new GetApplicationReviewsByVacancyReferenceQueryResult([]);
        }
    }
}