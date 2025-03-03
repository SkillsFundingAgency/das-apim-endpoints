using System.Net;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Recruit.Domain;
using SFA.DAS.Recruit.InnerApi.Requests;
using SFA.DAS.Recruit.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Recruit.Application.Candidates.Queries.GetCandidate;

public class GetCandidateQueryHandler(ICandidateApiClient<CandidateApiConfiguration> candidateApiClient) : IRequestHandler<GetCandidateQuery, GetCandidateQueryResult>
{
    public async Task<GetCandidateQueryResult> Handle(GetCandidateQuery request, CancellationToken cancellationToken)
    {
        var actual =
            await candidateApiClient.GetWithResponseCode<GetCandidateApiResponse>(
                new GetCandidateByIdApiRequest(request.CandidateId));

        if (actual.StatusCode == HttpStatusCode.NotFound)
        {
            return new GetCandidateQueryResult
            {
                Candidate = null
            };
        }

        return new GetCandidateQueryResult
        {
            Candidate = new Candidate
            {
                Id = actual.Body.Id,
                DateOfBirth = actual.Body.DateOfBirth
            }
        };
    }
}