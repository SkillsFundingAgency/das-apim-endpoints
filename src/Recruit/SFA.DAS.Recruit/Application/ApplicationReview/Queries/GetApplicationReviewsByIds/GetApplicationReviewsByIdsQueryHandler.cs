using MediatR;
using SFA.DAS.Recruit.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.Recruit.Application.ApplicationReview.Queries.GetApplicationReviewsByIds;
public class GetApplicationReviewsByIdsQueryHandler(
    IRecruitApiClient<RecruitApiConfiguration> recruitApiClient) :
    IRequestHandler<GetApplicationReviewsByIdsQuery, GetApplicationReviewsByIdsQueryResult>
{
    public async Task<GetApplicationReviewsByIdsQueryResult> Handle(GetApplicationReviewsByIdsQuery request, CancellationToken cancellationToken)
    {
        var recruitApiResponse = await recruitApiClient.PostWithResponseCode<List<Domain.ApplicationReview>>(
            new GetApplicationReviewsByIdsApiRequest(request.ApplicationIds));

        recruitApiResponse.EnsureSuccessStatusCode();

        if (recruitApiResponse == null) return new GetApplicationReviewsByIdsQueryResult();

        return new GetApplicationReviewsByIdsQueryResult
        {
            ApplicationReviews = recruitApiResponse.Body ?? []
        };
    }
}