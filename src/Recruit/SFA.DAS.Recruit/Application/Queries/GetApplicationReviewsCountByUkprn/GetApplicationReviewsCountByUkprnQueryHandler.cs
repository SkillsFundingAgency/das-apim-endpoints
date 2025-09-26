using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Recruit.InnerApi.Requests;
using SFA.DAS.Recruit.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Recruit.Application.Queries.GetApplicationReviewsCountByUkprn
{
    public class GetApplicationReviewsCountByUkprnQueryHandler(
        IRecruitApiClient<RecruitApiConfiguration> recruitApiClient) 
        : IRequestHandler<GetApplicationReviewsCountByUkprnQuery, GetApplicationReviewsCountByUkprnResult>
    {
        public async Task<GetApplicationReviewsCountByUkprnResult> Handle(GetApplicationReviewsCountByUkprnQuery request, CancellationToken cancellationToken)
        {
            var response = await recruitApiClient.PostWithResponseCode<List<ApplicationReviewStats>>(
                new GetApplicationReviewsCountByUkprnApiRequest(request.Ukprn, request.VacancyReferences));

            return new GetApplicationReviewsCountByUkprnResult
            {
                ApplicationReviewStatsList = response.Body ?? []
            };
        }
    }
}