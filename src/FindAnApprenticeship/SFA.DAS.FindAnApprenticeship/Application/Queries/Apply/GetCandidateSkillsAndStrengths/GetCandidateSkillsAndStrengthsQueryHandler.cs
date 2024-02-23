using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindAnApprenticeship.Application.Queries.Apply.GetCandidateSkillsAndStrengths;
public class GetCandidateSkillsAndStrengthsQueryHandler : IRequestHandler<GetCandidateSkillsAndStrengthsQuery, GetCandidateSkillsAndStrengthsQueryResult>
{
    private readonly ICandidateApiClient<CandidateApiConfiguration> _candidateApiClient;

    public GetCandidateSkillsAndStrengthsQueryHandler(ICandidateApiClient<CandidateApiConfiguration> candidateApiClient)
    {
        _candidateApiClient = candidateApiClient;
    }

    public async Task<GetCandidateSkillsAndStrengthsQueryResult> Handle(GetCandidateSkillsAndStrengthsQuery request, CancellationToken cancellationToken)
    {
        return await _candidateApiClient.Get<GetCandidateSkillsAndStrengthsItemApiResponse>
            (new GetCandidateSkillsAndStrengthsItemApiRequest(request.ApplicationId, request.CandidateId));
    }
}
