using System;
using System.Net;
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
            await _candidateApiClient.GetWithResponseCode<GetAboutYouItemApiResponse>(
                new GetAboutYouItemApiRequest(request.ApplicationId, request.CandidateId));

        if (response.StatusCode == HttpStatusCode.NotFound)
        {
            return await _candidateApiClient.PutWithResponseCode<PutUpsertAboutYouItemApiResponse>
            (new PutUpsertAboutYouItemApiRequest(request.ApplicationId, request.CandidateId, Guid.NewGuid(),
                new PutUpsertAboutYouItemApiRequest.PutUpdateAboutYouItemApiRequestData()));
        }
        
        return response;
    }
}
