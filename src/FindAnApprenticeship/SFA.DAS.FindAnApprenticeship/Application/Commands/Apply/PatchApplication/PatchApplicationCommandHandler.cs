using MediatR;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.FindAnApprenticeship.Application.Commands.Apply.PatchApplication
{
    public class PatchApplicationCommandHandler :IRequestHandler<PatchApplicationCommand, PatchApplicationCommandResponse>
    {
        private readonly ICandidateApiClient<CandidateApiConfiguration> _candidateApiClient;
        private readonly ILogger<PatchApplicationCommandHandler> _logger;

        public PatchApplicationCommandHandler(
            ICandidateApiClient<CandidateApiConfiguration> candidateApiClient,
            ILogger<PatchApplicationCommandHandler> logger)
        {
            _candidateApiClient = candidateApiClient;
            _logger = logger;
        }

        public async Task<PatchApplicationCommandResponse> Handle(PatchApplicationCommand request, CancellationToken cancellationToken)
        {
            var jsonPatchDocument = new JsonPatchDocument<Models.Application>();

            if (request.WorkExperienceStatus > 0)
            {
                jsonPatchDocument.Replace(x => x.WorkHistorySectionStatus, request.WorkExperienceStatus);
            }

            var patchRequest = new PatchApplicationApiRequest(request.ApplicationId, request.CandidateId, jsonPatchDocument);

            var response = await _candidateApiClient.PatchWithResponseCode(patchRequest);

            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                _logger.LogError($"Unable to patch application for candidate Id {request.CandidateId}");
            }

            return new PatchApplicationCommandResponse
            {
                Application = JsonConvert.DeserializeObject<Models.Application>(response.Body)
            };
        }
    }
}
