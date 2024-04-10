using MediatR;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SFA.DAS.FindAnApprenticeship.Application.Commands.Apply.PatchApplicationTrainingCourses;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Infrastructure;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.FindAnApprenticeship.Application.Commands.Apply.PatchApplicationDisabilityConfidence;

public record PatchApplicationDisabilityConfidenceCommandHandler : IRequestHandler<PatchApplicationDisabilityConfidenceCommand, PatchApplicationDisabilityConfidenceCommandResponse>
{
    private readonly ICandidateApiClient<CandidateApiConfiguration> _candidateApiClient;
    private readonly ILogger<PatchApplicationTrainingCoursesCommandHandler> _logger;

    public PatchApplicationDisabilityConfidenceCommandHandler(ICandidateApiClient<CandidateApiConfiguration> candidateApiClient, ILogger<PatchApplicationTrainingCoursesCommandHandler> logger)
    {
        _candidateApiClient = candidateApiClient;
        _logger = logger;
    }

    public async Task<PatchApplicationDisabilityConfidenceCommandResponse> Handle(PatchApplicationDisabilityConfidenceCommand request, CancellationToken cancellationToken)
    {
        var jsonPatchDocument = new JsonPatchDocument<Models.Application>();
        if (request.DisabilityConfidenceStatus > 0)
        {
            jsonPatchDocument.Replace(x => x.DisabilityConfidenceStatus, request.DisabilityConfidenceStatus);
        }

        var patchRequest = new PatchApplicationApiRequest(request.ApplicationId, request.CandidateId, jsonPatchDocument);
        var response = await _candidateApiClient.PatchWithResponseCode(patchRequest);
        if (response.StatusCode == System.Net.HttpStatusCode.OK)
            return new PatchApplicationDisabilityConfidenceCommandResponse
            {
                Application = JsonConvert.DeserializeObject<Models.Application>(response.Body)
            };

        _logger.LogError("Unable to patch application for candidate Id {request.CandidateId}", request.CandidateId);
        throw new HttpRequestContentException($"Unable to patch application for candidate Id {request.CandidateId}", response.StatusCode, response.ErrorContent);
    }
}