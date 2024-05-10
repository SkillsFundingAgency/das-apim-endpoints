using MediatR;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SFA.DAS.FindAnApprenticeship.Application.Commands.Apply.PatchApplicationTrainingCourses;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Infrastructure;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Threading.Tasks;
using System.Threading;

namespace SFA.DAS.FindAnApprenticeship.Application.Commands.Apply.PatchApplicationStatus;

public record PatchApplicationStatusCommandHandler(
    ICandidateApiClient<CandidateApiConfiguration> CandidateApiClient,
    ILogger<PatchApplicationTrainingCoursesCommandHandler> Logger)
    : IRequestHandler<PatchApplicationStatusCommand, PatchApplicationStatusCommandResponse>
{
    public async Task<PatchApplicationStatusCommandResponse> Handle(PatchApplicationStatusCommand request, CancellationToken cancellationToken)
    {
        var jsonPatchDocument = new JsonPatchDocument<Models.Application>();
        if (request.Status > 0)
        {
            jsonPatchDocument.Replace(x => x.Status, request.Status);
        }

        var patchRequest = new PatchApplicationApiRequest(request.ApplicationId, request.CandidateId, jsonPatchDocument);
        var response = await CandidateApiClient.PatchWithResponseCode(patchRequest);
        if (response.StatusCode == System.Net.HttpStatusCode.OK)
            return new PatchApplicationStatusCommandResponse
            {
                Application = JsonConvert.DeserializeObject<Models.Application>(response.Body)
            };

        Logger.LogError("Unable to patch application for candidate Id {request.CandidateId}", request.CandidateId);
        throw new HttpRequestContentException($"Unable to patch application for candidate Id {request.CandidateId}", response.StatusCode, response.ErrorContent);
    }
}