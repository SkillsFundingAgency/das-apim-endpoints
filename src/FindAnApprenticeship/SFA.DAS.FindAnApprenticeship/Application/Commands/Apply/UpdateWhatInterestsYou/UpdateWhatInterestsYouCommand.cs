using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SFA.DAS.FindAnApprenticeship.Application.Commands.Apply.PatchApplicationTrainingCourses;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;
using SFA.DAS.FindAnApprenticeship.Models;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Infrastructure;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindAnApprenticeship.Application.Commands.Apply.UpdateWhatInterestsYou
{
    public class UpdateWhatInterestsYouCommand : IRequest
    {
        public Guid ApplicationId { get; set; }
        public Guid CandidateId { get; set; }
        public string AnswerText { get; set; }
        public bool IsComplete { get; set; }
    }

    public class UpdateWhatInterestsYouCommandHandler : IRequestHandler<UpdateWhatInterestsYouCommand>
    {
        private readonly ICandidateApiClient<CandidateApiConfiguration> _candidateApiClient;
        private readonly ILogger<UpdateWhatInterestsYouCommandHandler> _logger;

        public UpdateWhatInterestsYouCommandHandler(ICandidateApiClient<CandidateApiConfiguration> candidateApiClient, ILogger<UpdateWhatInterestsYouCommandHandler> logger)
        {
            _candidateApiClient = candidateApiClient;
            _logger = logger;
        }

        public async Task<Unit> Handle(UpdateWhatInterestsYouCommand request, CancellationToken cancellationToken)
        {
            var jsonPatchDocument = new JsonPatchDocument<Models.Application>();
            jsonPatchDocument.Replace(x => x.InterestsStatus, request.IsComplete ? SectionStatus.Completed : SectionStatus.InProgress);

            var patchRequest = new PatchApplicationApiRequest(request.ApplicationId, request.CandidateId, jsonPatchDocument);
            var response = await _candidateApiClient.PatchWithResponseCode(patchRequest);
            
            if (response.StatusCode == System.Net.HttpStatusCode.OK) return Unit.Value;

            _logger.LogError($"Unable to patch application for candidate Id {request.CandidateId}");
            throw new HttpRequestContentException(
                $"Unable to patch application for candidate Id {request.CandidateId}", response.StatusCode,
                response.ErrorContent);
        }
    }
}
