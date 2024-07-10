using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;
using SFA.DAS.SharedOuterApi.Configuration;
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
        var jsonPatchDocument = new JsonPatchDocument<Domain.Models.Application>();
        jsonPatchDocument.Replace(x => x.Strengths, command.SkillsAndStrengths);
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

        var resultBody = JsonConvert.DeserializeObject<Domain.Models.Application>(patchResult.Body);
        return new UpsertSkillsAndStrengthsCommandResult
        {
            Id = resultBody.Id,
            Application = resultBody
        };
    }
}
