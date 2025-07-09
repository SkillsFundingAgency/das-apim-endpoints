using MediatR;
using SFA.DAS.Recruit.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.Recruit.Application.ApplicationReview.Queries.GetApplicationReviewsByVacancyReference
{
    public class GetApplicationReviewsByVacancyReferenceQueryHandler(IRecruitApiClient<RecruitApiConfiguration> recruitApiClient) : IRequestHandler<GetApplicationReviewsByVacancyReferenceQuery, GetApplicationReviewsByVacancyReferenceQueryResult>
    {
        public async Task<GetApplicationReviewsByVacancyReferenceQueryResult> Handle(GetApplicationReviewsByVacancyReferenceQuery request, CancellationToken cancellationToken)
        {
            var response = await recruitApiClient.Get<List<InnerApi.Responses.ApplicationReview>>(
                new GetApplicationReviewsByVacancyReferenceApiRequest(request.VacancyReference));

            return new GetApplicationReviewsByVacancyReferenceQueryResult(response);
        }
    }
}