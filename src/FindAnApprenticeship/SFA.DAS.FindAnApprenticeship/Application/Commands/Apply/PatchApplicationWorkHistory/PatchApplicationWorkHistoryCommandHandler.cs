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

namespace SFA.DAS.FindAnApprenticeship.Application.Commands.Apply.PatchApplication
{
    public class PatchApplicationWorkHistoryCommandHandler :IRequestHandler<PatchApplicationWorkHistoryCommand, PatchApplicationWorkHistoryCommandResponse>
    {
        private readonly ICandidateApiClient<CandidateApiConfiguration> _candidateApiClient;
        private readonly ILogger<PatchApplicationWorkHistoryCommandHandler> _logger;

        public PatchApplicationWorkHistoryCommandHandler(
            ICandidateApiClient<CandidateApiConfiguration> candidateApiClient,
            ILogger<PatchApplicationWorkHistoryCommandHandler> logger)
        {
            _candidateApiClient = candidateApiClient;
            _logger = logger;
        }

        public async Task<PatchApplicationWorkHistoryCommandResponse> Handle(PatchApplicationWorkHistoryCommand request, CancellationToken cancellationToken)
        {
            var jsonPatchDocument = new JsonPatchDocument<Models.Application>();
            if (request.WorkExperienceStatus > 0)
            {
                //jsonPatchDocument.Replace(x => x.WorkExperienceStatus, request.WorkExperienceStatus);
                jsonPatchDocument.Replace(x => x.JobsStatus, request.WorkExperienceStatus);
            }

            var patchRequest = new PatchApplicationApiRequest(request.ApplicationId, request.CandidateId, jsonPatchDocument);
            var response = await _candidateApiClient.PatchWithResponseCode(patchRequest);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
                return new PatchApplicationWorkHistoryCommandResponse
                {
                    Application = JsonConvert.DeserializeObject<Models.Application>(response.Body)
                };

            _logger.LogError($"Unable to patch application for candidate Id {request.CandidateId}");
            throw new HttpRequestContentException($"Unable to patch application for candidate Id {request.CandidateId}", response.StatusCode, response.ErrorContent);
        }
    }
}
