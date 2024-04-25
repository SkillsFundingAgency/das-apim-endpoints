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
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.FindAnApprenticeship.Application.Commands.Apply.CreateInterviewAdjustments;
public class UpsertInterviewAdjustmentsCommandHandler : IRequestHandler<UpsertInterviewAdjustmentsCommand, UpsertInterviewAdjustmentsCommandResult>
{
    private readonly ICandidateApiClient<CandidateApiConfiguration> _apiClient;
    private readonly ILogger<UpsertInterviewAdjustmentsCommandHandler> _logger;

    public UpsertInterviewAdjustmentsCommandHandler(
        ICandidateApiClient<CandidateApiConfiguration> apiClient,
        ILogger<UpsertInterviewAdjustmentsCommandHandler> logger)
    {
        _apiClient = apiClient;
        _logger = logger;
    }

    public async Task<UpsertInterviewAdjustmentsCommandResult> Handle(UpsertInterviewAdjustmentsCommand command, CancellationToken cancellationToken)
    {
        var jsonPatchDocument = new JsonPatchDocument<Models.Application>();
        if (command.InterviewAdjustmentsSectionStatus > 0)
        {
            jsonPatchDocument.Replace(x => x.InterviewAdjustmentsStatus, command.InterviewAdjustmentsSectionStatus);
        }

        var patchRequest = new PatchApplicationApiRequest(command.ApplicationId, command.CandidateId, jsonPatchDocument);
        var patchResult = await _apiClient.PatchWithResponseCode(patchRequest);
        if (patchResult.StatusCode != System.Net.HttpStatusCode.OK)
        {
            _logger.LogError("Unable to patch application for candidate Id {CandidateId}", command.CandidateId);
            throw new HttpRequestContentException($"Unable to patch application for candidate Id {command.CandidateId}", patchResult.StatusCode, patchResult.ErrorContent);
        }

        var aboutYouItem = await _apiClient.Get<GetAboutYouItemApiResponse>(new GetAboutYouItemApiRequest(command.ApplicationId, command.CandidateId));

        var requestBody = new PutUpsertAboutYouItemApiRequest.PutUpdateAboutYouItemApiRequestData
        {
            SkillsAndStrengths = aboutYouItem.AboutYou?.SkillsAndStrengths,
            HobbiesAndInterests = aboutYouItem.AboutYou?.HobbiesAndInterests,
            Improvements = aboutYouItem.AboutYou?.Improvements,
            Sex = aboutYouItem.AboutYou?.Sex,
            EthnicGroup = aboutYouItem.AboutYou?.EthnicGroup,
            EthnicSubGroup = aboutYouItem.AboutYou?.EthnicSubGroup,
            IsGenderIdentifySameSexAtBirth = aboutYouItem.AboutYou?.IsGenderIdentifySameSexAtBirth,
            OtherEthnicSubGroupAnswer = aboutYouItem.AboutYou?.OtherEthnicSubGroupAnswer,
            Support = command.InterviewAdjustmentsDescription,
        };
        var request = new PutUpsertAboutYouItemApiRequest(command.ApplicationId, command.CandidateId, Guid.NewGuid(), requestBody);

        var putResult = await _apiClient.PutWithResponseCode<PutUpsertAboutYouItemApiResponse>(request);
        putResult.EnsureSuccessStatusCode();

        if (putResult is null) return null;

        return new UpsertInterviewAdjustmentsCommandResult
        {
            Id = putResult.Body.Id,
            Application = JsonConvert.DeserializeObject<Models.Application>(patchResult.Body)
        };
    }
}
