using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.Infrastructure;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindAnApprenticeship.Application.Commands.Apply.CreateSkillsAndStrengthsCommand;
public class UpsertSkillsAndStrengthsCommandHandler : IRequestHandler<UpsertSkillsAndStrengthsCommand, UpsertSkillsAndStrengthsCommandResult>
{
    private readonly ICandidateApiClient<CandidateApiConfiguration> _apiClient;
    private readonly ILogger<UpsertSkillsAndStrengthsCommandHandler> _logger;

    public UpsertSkillsAndStrengthsCommandHandler(ICandidateApiClient<CandidateApiConfiguration> apiClient, ILogger<UpsertSkillsAndStrengthsCommandHandler> logger)
    {
        _apiClient = apiClient;
        _logger = logger;
    }

    public async Task<UpsertSkillsAndStrengthsCommandResult> Handle(UpsertSkillsAndStrengthsCommand command, CancellationToken cancellationToken)
    {
        var jsonPatchDocument = new JsonPatchDocument<Models.Application>();
        if (command.SkillsAndStrengthsSectionStatus > 0)
        {
            jsonPatchDocument.Replace(x => x.SkillsAndStrengthStatus, command.SkillsAndStrengthsSectionStatus);
        }

        var patchRequest = new PatchApplicationApiRequest(command.ApplicationId, command.CandidateId, jsonPatchDocument);
        var patchResult = await _apiClient.PatchWithResponseCode(patchRequest);
        if (patchResult.StatusCode != System.Net.HttpStatusCode.OK)
        {
            _logger.LogError("Unable to patch application for candidate Id {command.CandidateId}", command.CandidateId);
            throw new HttpRequestContentException($"Unable to patch application for candidate Id {command.CandidateId}", patchResult.StatusCode, patchResult.ErrorContent);
        }

        var requestBody = new PutUpsertSkillsAndStrengthsApiRequest.PutUpdateSkillsAndStrengthsApiRequestData
        {
            SkillsAndStrengths = command.SkillsAndStrengths
        };
        var request = new PutUpsertSkillsAndStrengthsApiRequest(command.ApplicationId, command.CandidateId, Guid.NewGuid(), requestBody);

        var putResult = await _apiClient.PutWithResponseCode<PutUpsertSkillsAndStrengthsApiResponse>(request);
        putResult.EnsureSuccessStatusCode();

        if (putResult is null) return null;

        return new UpsertSkillsAndStrengthsCommandResult
        {
            Id = putResult.Body.Id,
            Application = JsonConvert.DeserializeObject<Models.Application>(patchResult.Body)
        };
    }
}
