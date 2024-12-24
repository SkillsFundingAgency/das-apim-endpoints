using MediatR;
using SFA.DAS.FindApprenticeshipJobs.InnerApi.Requests;
using SFA.DAS.FindApprenticeshipJobs.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindApprenticeshipJobs.Application.Queries.SavedSearch.GetInactiveCandidates
{
    public class GetInactiveCandidatesQueryHandler(
        ICandidateApiClient<CandidateApiConfiguration> candidateApiClient)
        : IRequestHandler<GetInactiveCandidatesQuery, GetInactiveCandidatesQueryResult>
    {
        public async Task<GetInactiveCandidatesQueryResult> Handle(GetInactiveCandidatesQuery request, CancellationToken cancellationToken)
        {
            var candidatesResponse = await candidateApiClient.Get<GetInactiveCandidatesApiResponse>(
                new GetInactiveCandidatesApiRequest(
                    request.CutOffDateTime.ToString("O"),
                    request.PageNumber,
                    request.PageSize));

            if (candidatesResponse is not { Candidates.Count: > 0 })
                return new GetInactiveCandidatesQueryResult
                {
                    PageSize = candidatesResponse.PageSize,
                    PageIndex = candidatesResponse.PageIndex,
                    TotalPages = candidatesResponse.TotalPages,
                    TotalCount = candidatesResponse.TotalCount,
                    Candidates = []
                };

            return new GetInactiveCandidatesQueryResult
            {
                Candidates = candidatesResponse.Candidates.Select(candidate => (GetInactiveCandidatesQueryResult.Candidate)candidate).ToList(),
                PageSize = candidatesResponse.PageSize,
                PageIndex = candidatesResponse.PageIndex,
                TotalPages = candidatesResponse.TotalPages,
                TotalCount = candidatesResponse.TotalCount
            };
        }
    }
}