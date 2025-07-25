using MediatR;
using SFA.DAS.Recruit.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.Recruit.Application.ApplicationReview.Queries.GetApplicationReviewById;
public class GetApplicationReviewByIdQueryHandler(
    IRecruitApiClient<RecruitApiConfiguration> recruitApiClient,
    ICandidateApiClient<CandidateApiConfiguration> candidateApiClient) : IRequestHandler<GetApplicationReviewByIdQuery, GetApplicationReviewByIdQueryResult>
{
    public async Task<GetApplicationReviewByIdQueryResult> Handle(GetApplicationReviewByIdQuery request, CancellationToken cancellationToken)
    {
        var recruitApiResponse = await recruitApiClient.Get<Domain.ApplicationReview>(
            new GetApplicationReviewByIdApiRequest(request.ApplicationReviewId));

        if (recruitApiResponse == null) return new GetApplicationReviewByIdQueryResult();

        if (recruitApiResponse.ApplicationId != null)
        {
            var candidateApiResponse = await candidateApiClient.Get<Domain.Application>(
                new GetApplicationByIdApiRequest(recruitApiResponse.ApplicationId.Value, recruitApiResponse.CandidateId));
            recruitApiResponse.Application = candidateApiResponse;
        }

        return new GetApplicationReviewByIdQueryResult
        {
            ApplicationReview = recruitApiResponse
        };
    }
}