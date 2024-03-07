using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.Extensions.Logging;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Infrastructure;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindAnApprenticeship.Application.Commands.Apply.DisabilityConfident
{
    public class UpdateDisabilityConfidentCommand : IRequest
    {
        public Guid CandidateId { get; set; }
        public Guid ApplicationId { get; set; }
        public bool ApplyUnderDisabilityConfidentScheme { get; set; }
    }

    public class UpdateDisabilityConfidentCommandHandler(ICandidateApiClient<CandidateApiConfiguration> candidateApiClient, ILogger<UpdateDisabilityConfidentCommandHandler> logger) : IRequestHandler<UpdateDisabilityConfidentCommand>
    {
        public async Task Handle(UpdateDisabilityConfidentCommand request, CancellationToken cancellationToken)
        {
            var jsonPatchDocument = new JsonPatchDocument<Models.Application>();
            
            jsonPatchDocument.Replace(x => x.ApplyUnderDisabilityConfidentScheme, request.ApplyUnderDisabilityConfidentScheme);

            var patchRequest = new PatchApplicationApiRequest(request.ApplicationId, request.CandidateId, jsonPatchDocument);
            var response = await candidateApiClient.PatchWithResponseCode(patchRequest);

            if (response.StatusCode == System.Net.HttpStatusCode.OK) return;

            logger.LogError($"Unable to patch application for candidate Id {request.CandidateId}");
            throw new HttpRequestContentException(
                $"Unable to patch application for candidate Id {request.CandidateId}", response.StatusCode,
                response.ErrorContent);
        }
    }

}
