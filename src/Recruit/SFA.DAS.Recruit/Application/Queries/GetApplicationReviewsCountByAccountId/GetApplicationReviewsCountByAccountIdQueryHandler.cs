using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Recruit.InnerApi.Requests;
using SFA.DAS.Recruit.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Recruit.Application.Queries.GetApplicationReviewsCountByAccountId
{
    public class GetApplicationReviewsCountByAccountIdQueryHandler(
        IRecruitApiClient<RecruitApiConfiguration> recruitApiClient) 
        : IRequestHandler<GetApplicationReviewsCountByAccountIdQuery, GetApplicationReviewsCountByAccountIdQueryResult>
    {
        public async Task<GetApplicationReviewsCountByAccountIdQueryResult> Handle(GetApplicationReviewsCountByAccountIdQuery request, CancellationToken cancellationToken)
        {
            var response = await recruitApiClient.PostWithResponseCode<List<ApplicationReviewStats>>(
                new GetApplicationReviewsCountByAccountIdApiRequest(request.AccountId, request.VacancyReferences, request.ApplicationSharedFilteringStatus));

            return new GetApplicationReviewsCountByAccountIdQueryResult
            {
                ApplicationReviewStatsList = response.Body ?? []
            };
        }
    }
}