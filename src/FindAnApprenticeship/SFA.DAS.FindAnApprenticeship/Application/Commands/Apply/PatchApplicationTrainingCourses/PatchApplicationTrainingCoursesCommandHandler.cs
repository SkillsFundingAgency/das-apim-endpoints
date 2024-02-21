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

namespace SFA.DAS.FindAnApprenticeship.Application.Commands.Apply.PatchApplicationTrainingCourses;
public class PatchApplicationTrainingCoursesCommandHandler : IRequestHandler<PatchApplicationTrainingCoursesCommand, PatchApplicationTrainingCoursesCommandResponse>
{
    private readonly ICandidateApiClient<CandidateApiConfiguration> _candidateApiClient;
    private readonly ILogger<PatchApplicationTrainingCoursesCommandHandler> _logger;

    public PatchApplicationTrainingCoursesCommandHandler(ICandidateApiClient<CandidateApiConfiguration> candidateApiClient, ILogger<PatchApplicationTrainingCoursesCommandHandler> logger)
    {
        _candidateApiClient = candidateApiClient;
        _logger = logger;
    }

    public async Task<PatchApplicationTrainingCoursesCommandResponse> Handle(PatchApplicationTrainingCoursesCommand request, CancellationToken cancellationToken)
    {
        var jsonPatchDocument = new JsonPatchDocument<Models.Application>();
        if (request.TrainingCoursesStatus > 0)
        {
            jsonPatchDocument.Replace(x => x.TrainingCoursesStatus, request.TrainingCoursesStatus);
        }

        var patchRequest = new PatchApplicationApiRequest(request.ApplicationId, request.CandidateId, jsonPatchDocument);
        var response = await _candidateApiClient.PatchWithResponseCode(patchRequest);
        if (response.StatusCode == System.Net.HttpStatusCode.OK)
            return new PatchApplicationTrainingCoursesCommandResponse
            {
                Application = JsonConvert.DeserializeObject<Models.Application>(response.Body)
            };

        _logger.LogError($"Unable to patch application for candidate Id {request.CandidateId}");
        throw new HttpRequestContentException($"Unable to patch application for candidate Id {request.CandidateId}", response.StatusCode, response.ErrorContent);
    }
}
