using MediatR;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Infrastructure;
using SFA.DAS.SharedOuterApi.Interfaces;
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
        var jsonPatchDocument = new JsonPatchDocument<Domain.Models.Application>();
        jsonPatchDocument.Replace(x => x.Support, command.InterviewAdjustmentsDescription);
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

        var patchResultBody = JsonConvert.DeserializeObject<Domain.Models.Application>(patchResult.Body);

        return new UpsertInterviewAdjustmentsCommandResult
        {
            Id = patchResultBody.Id,
            Application = patchResultBody
        };
    }
}
