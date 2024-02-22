using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindAnApprenticeship.Application.Commands.Apply.CreateSkillsAndStrengthsCommand;
public class CreateSkillsAndStrengthsCommandHandler : IRequestHandler<CreateSkillsAndStrengthsCommand, CreateSkillsAndStrengthsCommandResult>
{
    private readonly ICandidateApiClient<CandidateApiConfiguration> _apiClient;

    public CreateSkillsAndStrengthsCommandHandler(ICandidateApiClient<CandidateApiConfiguration> apiClient)
    {
        _apiClient = apiClient;
    }

    public async Task<CreateSkillsAndStrengthsCommandResult> Handle(CreateSkillsAndStrengthsCommand command, CancellationToken cancellationToken)
    {
        var requestBody = new PutUpsertSkillsAndStrengthsApiRequest.PutUpdateSkillsAndStrengthsApiRequestData
        {
            SkillsAndStrengths = command.SkillsAndStrengths
        };
        var request = new PutUpsertSkillsAndStrengthsApiRequest(command.ApplicationId, command.CandidateId, Guid.NewGuid(), requestBody);

        var result = await _apiClient.PutWithResponseCode<PutUpsertSkillsAndStrengthsApiResponse>(request);
        result.EnsureSuccessStatusCode();

        if (result is null) return null;

        return new CreateSkillsAndStrengthsCommandResult
        {
            Id = result.Body.Id
        };
    }
}
