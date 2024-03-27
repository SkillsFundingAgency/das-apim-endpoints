using MediatR;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.Extensions.Logging;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Infrastructure;
using SFA.DAS.SharedOuterApi.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.FindAnApprenticeship.Models;

namespace SFA.DAS.FindAnApprenticeship.Application.Commands.Apply.UpdateQualifications
{
    public class UpdateQualificationsCommand : IRequest
    {
        public Guid ApplicationId { get; set; }
        public Guid CandidateId { get; set; }
        public bool IsComplete { get; set; }
    }

    public class UpdateQualificationsCommandHandler : IRequestHandler<UpdateQualificationsCommand>
    {
        private readonly ICandidateApiClient<CandidateApiConfiguration> _candidateApiClient;
        private readonly ILogger<UpdateQualificationsCommandHandler> _logger;

        public UpdateQualificationsCommandHandler(ICandidateApiClient<CandidateApiConfiguration> candidateApiClient, ILogger<UpdateQualificationsCommandHandler> logger)
        {
            _candidateApiClient = candidateApiClient;
            _logger = logger;
        }

        public async Task Handle(UpdateQualificationsCommand request, CancellationToken cancellationToken)
        {
            var jsonPatchDocument = new JsonPatchDocument<Models.Application>();
            jsonPatchDocument.Replace(x => x.QualificationsStatus, request.IsComplete ? SectionStatus.Completed : SectionStatus.Incomplete);

            var patchRequest = new PatchApplicationApiRequest(request.ApplicationId, request.CandidateId, jsonPatchDocument);
            var response = await _candidateApiClient.PatchWithResponseCode(patchRequest);

            if (response.StatusCode == System.Net.HttpStatusCode.OK) return;

            _logger.LogError($"Unable to patch application for candidate Id {request.CandidateId}");
            throw new HttpRequestContentException(
                $"Unable to patch application for candidate Id {request.CandidateId}", response.StatusCode,
                response.ErrorContent);
        }
    }
}
