using MediatR;
using SFA.DAS.Recruit.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.Recruit.Application.ApplicationReview.Queries.GetApplicationReviewsByIds;
public class GetApplicationReviewsByIdsQueryHandler(
    IRecruitApiClient<RecruitApiConfiguration> recruitApiClient, ICandidateApiClient<CandidateApiConfiguration> candidateApiClient)
    : IRequestHandler<GetApplicationReviewsByIdsQuery, GetApplicationReviewsByIdsQueryResult>
{
    public async Task<GetApplicationReviewsByIdsQueryResult> Handle(GetApplicationReviewsByIdsQuery request, CancellationToken cancellationToken)
    {
        var recruitApiResponse = await recruitApiClient.PostWithResponseCode<List<Domain.ApplicationReview>>(
            new GetApplicationReviewsByIdsApiRequest(request.ApplicationIds));

        // If the API itself failed or returned nothing
        if (recruitApiResponse?.Body == null ||
            recruitApiResponse.Body.Count == 0)
        {
            return new GetApplicationReviewsByIdsQueryResult
            {
                ApplicationReviews = []
            };
        }

        foreach (var applicationReview in recruitApiResponse.Body)
        {
            if (applicationReview?.ApplicationId == null)
                continue;

            var candidateApiResponse = await candidateApiClient.Get<Domain.Application>(
                new GetApplicationByIdApiRequest(
                    applicationReview.ApplicationId.Value,
                    applicationReview.CandidateId));

            var target = recruitApiResponse.Body
                .FirstOrDefault(x => x.ApplicationId == applicationReview.ApplicationId);

            if (target != null)
                target.Application = candidateApiResponse;
        }

        return new GetApplicationReviewsByIdsQueryResult
        {
            ApplicationReviews = recruitApiResponse.Body ?? []
        };
    }
}