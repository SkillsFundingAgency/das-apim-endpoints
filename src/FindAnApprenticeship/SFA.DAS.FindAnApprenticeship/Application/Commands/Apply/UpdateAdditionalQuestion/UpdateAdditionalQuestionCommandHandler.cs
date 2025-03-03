﻿using System.Threading;
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

namespace SFA.DAS.FindAnApprenticeship.Application.Commands.Apply.UpdateAdditionalQuestion;
public class UpdateAdditionalQuestionCommandHandler : IRequestHandler<UpdateAdditionalQuestionCommand, UpdateAdditionalQuestionQueryResult>
{
    private readonly ICandidateApiClient<CandidateApiConfiguration> _apiClient;
    private readonly ILogger<UpdateAdditionalQuestionCommandHandler> _logger;

    public UpdateAdditionalQuestionCommandHandler(
        ICandidateApiClient<CandidateApiConfiguration> apiClient,
        ILogger<UpdateAdditionalQuestionCommandHandler> logger)
    {
        _apiClient = apiClient;
        _logger = logger;
    }

    public async Task<UpdateAdditionalQuestionQueryResult> Handle(UpdateAdditionalQuestionCommand command, CancellationToken cancellationToken)
    {
        var jsonPatchDocument = new JsonPatchDocument<Domain.Models.Application>();

        switch (command.UpdatedAdditionalQuestion)
        {
            case 1:
                jsonPatchDocument.Replace(x => x.AdditionalQuestion1Status, command.AdditionalQuestionSectionStatus);
                break;

            case 2:
                jsonPatchDocument.Replace(x => x.AdditionalQuestion2Status, command.AdditionalQuestionSectionStatus);
                break;

            default:
                break;
        }

        var patchRequest = new PatchApplicationApiRequest(command.ApplicationId, command.CandidateId, jsonPatchDocument);
        var patchResult = await _apiClient.PatchWithResponseCode(patchRequest);
        if (patchResult.StatusCode != System.Net.HttpStatusCode.OK)
        {
            _logger.LogError($"Unable to patch application for candidate Id {command.CandidateId}");
            throw new HttpRequestContentException($"Unable to patch application for candidate Id {command.CandidateId}", patchResult.StatusCode, patchResult.ErrorContent);
        }

        var requestBody = new PutUpsertAdditionalQuestionApiRequest.PutUpsertAdditionalQuestionApiRequestData
        {
            Answer = command.Answer
        };
        var request = new PutUpsertAdditionalQuestionApiRequest(command.ApplicationId, command.CandidateId, command.Id, requestBody);

        var upsertResult = await _apiClient.PutWithResponseCode<PutUpsertAdditionalQuestionApiResponse>(request);
        upsertResult.EnsureSuccessStatusCode();

        if (upsertResult is null) return null;

        return new UpdateAdditionalQuestionQueryResult
        {
            Application = JsonConvert.DeserializeObject<Domain.Models.Application>(patchResult.Body),
            Id = upsertResult.Body.Id
        };
    }
}
