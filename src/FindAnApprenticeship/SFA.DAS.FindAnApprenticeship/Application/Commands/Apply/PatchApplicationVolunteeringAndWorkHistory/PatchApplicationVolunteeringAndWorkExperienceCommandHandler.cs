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

namespace SFA.DAS.FindAnApprenticeship.Application.Commands.Apply.PatchApplicationVolunteeringAndWorkHistory;
public class PatchApplicationVolunteeringAndWorkExperienceCommandHandler : IRequestHandler<PatchApplicationVolunteeringAndWorkExperienceCommand, PatchApplicationVolunteeringAndWorkExperienceCommandResponse>
{
    private readonly ICandidateApiClient<CandidateApiConfiguration> _candidateApiClient;
    private readonly ILogger<PatchApplicationVolunteeringAndWorkExperienceCommandHandler> _logger;

    public PatchApplicationVolunteeringAndWorkExperienceCommandHandler(ICandidateApiClient<CandidateApiConfiguration> candidateApiClient, ILogger<PatchApplicationVolunteeringAndWorkExperienceCommandHandler> logger)
    {
        _candidateApiClient = candidateApiClient;
        _logger = logger;
    }

    public async Task<PatchApplicationVolunteeringAndWorkExperienceCommandResponse> Handle(PatchApplicationVolunteeringAndWorkExperienceCommand request, CancellationToken cancellationToken)
    {
        var jsonPatchDocument = new JsonPatchDocument<Models.Application>();
        if (request.VolunteeringAndWorkExperienceStatus > 0)
        {
            jsonPatchDocument.Replace(x => x.WorkExperienceStatus, request.VolunteeringAndWorkExperienceStatus);
        }

        var patchRequest = new PatchApplicationApiRequest(request.ApplicationId, request.CandidateId, jsonPatchDocument);
        var response = await _candidateApiClient.PatchWithResponseCode(patchRequest);
        if (response.StatusCode == System.Net.HttpStatusCode.OK)
            return new PatchApplicationVolunteeringAndWorkExperienceCommandResponse
            {
                Application = JsonConvert.DeserializeObject<Models.Application>(response.Body)
            };

        _logger.LogError($"Unable to patch application for candidate Id {request.CandidateId}");
        throw new HttpRequestContentException($"Unable to patch application for candidate Id {request.CandidateId}", response.StatusCode, response.ErrorContent);
    }
}
