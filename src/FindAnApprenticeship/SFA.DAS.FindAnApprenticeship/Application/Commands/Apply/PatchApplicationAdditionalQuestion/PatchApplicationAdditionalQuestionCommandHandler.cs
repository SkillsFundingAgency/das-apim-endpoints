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

namespace SFA.DAS.FindAnApprenticeship.Application.Commands.Apply.PatchApplicationAdditionalQuestion;

public record PatchApplicationAdditionalQuestionCommandHandler : IRequestHandler<PatchApplicationAdditionalQuestionCommand, PatchApplicationAdditionalQuestionCommandResponse>
{
    private readonly ICandidateApiClient<CandidateApiConfiguration> _candidateApiClient;
    private readonly ILogger<PatchApplicationAdditionalQuestionCommandHandler> _logger;

    public PatchApplicationAdditionalQuestionCommandHandler(
        ICandidateApiClient<CandidateApiConfiguration> candidateApiClient,
        ILogger<PatchApplicationAdditionalQuestionCommandHandler> logger)
    {
        _candidateApiClient = candidateApiClient;
        _logger = logger;
    }

    public async Task<PatchApplicationAdditionalQuestionCommandResponse> Handle(PatchApplicationAdditionalQuestionCommand request, CancellationToken cancellationToken)
    {
        var jsonPatchDocument = new JsonPatchDocument<Models.Application>();
        if (request.AdditionalQuestionOne > 0)
        {
            jsonPatchDocument.Replace(x => x.AdditionalQuestion1Status, request.AdditionalQuestionOne);
        }
        if (request.AdditionalQuestionTwo > 0)
        {
            jsonPatchDocument.Replace(x => x.AdditionalQuestion2Status, request.AdditionalQuestionTwo);
        }

        var patchRequest = new PatchApplicationApiRequest(request.ApplicationId, request.CandidateId, jsonPatchDocument);
        var response = await _candidateApiClient.PatchWithResponseCode(patchRequest);
        if (response.StatusCode == System.Net.HttpStatusCode.OK)
            return new PatchApplicationAdditionalQuestionCommandResponse
            {
                Application = JsonConvert.DeserializeObject<Models.Application>(response.Body)
            };

        _logger.LogError($"Unable to patch application for candidate Id {request.CandidateId}");
        throw new HttpRequestContentException($"Unable to patch application for candidate Id {request.CandidateId}", response.StatusCode, response.ErrorContent);
    }
}