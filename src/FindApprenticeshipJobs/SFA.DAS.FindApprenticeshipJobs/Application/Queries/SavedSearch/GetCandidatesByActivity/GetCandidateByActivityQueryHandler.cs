using MediatR;
using SFA.DAS.FindApprenticeshipJobs.InnerApi.Requests;
using SFA.DAS.FindApprenticeshipJobs.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindApprenticeshipJobs.Application.Queries.SavedSearch.GetCandidatesByActivity
{
    public class GetCandidateByActivityQueryHandler(
        ICandidateApiClient<CandidateApiConfiguration> candidateApiClient)
        : IRequestHandler<GetCandidateByActivityQuery, GetCandidateByActivityQueryResult>
    {
        public async Task<GetCandidateByActivityQueryResult> Handle(GetCandidateByActivityQuery request, CancellationToken cancellationToken)
        {
            var candidatesResponse = await candidateApiClient.Get<GetCandidatesByActivityApiResponse>(
                new GetCandidatesByActivityApiRequest(
                    request.CutOffDateTime.ToString("O"),
                    request.PageNumber,
                    request.PageSize));

            if (candidatesResponse is not { Candidates.Count: > 0 })
                return new GetCandidateByActivityQueryResult
                {
                    PageSize = candidatesResponse.PageSize,
                    PageIndex = candidatesResponse.PageIndex,
                    TotalPages = candidatesResponse.TotalPages,
                    TotalCount = candidatesResponse.TotalCount,
                    Candidates = []
                };

            return new GetCandidateByActivityQueryResult
            {
                Candidates = candidatesResponse.Candidates.Select(candidate => (GetCandidateByActivityQueryResult.Candidate)candidate).ToList(),
                PageSize = candidatesResponse.PageSize,
                PageIndex = candidatesResponse.PageIndex,
                TotalPages = candidatesResponse.TotalPages,
                TotalCount = candidatesResponse.TotalCount
            };
        }
    }
}