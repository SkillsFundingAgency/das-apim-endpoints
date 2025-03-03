using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses;
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
        var response =
            await _candidateApiClient.GetWithResponseCode<GetApplicationApiResponse>(
                new GetApplicationApiRequest(request.CandidateId, request.ApplicationId, false));

        if (response == null) return null;

        return new GetCandidateSkillsAndStrengthsQueryResult
        {
            Strengths = response.Body.Strengths
        };
    }
}
