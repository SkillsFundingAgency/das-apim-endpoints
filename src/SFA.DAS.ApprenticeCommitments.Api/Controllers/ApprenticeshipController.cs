using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApprenticeCommitments.Apis.CommitmentsV2InnerApi;
using SFA.DAS.ApprenticeCommitments.Application.Services;
using System;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeCommitments.Api.Controllers
{
    [ApiController]
    public class ApprenticeshipController : ControllerBase
    {
        private readonly ResponseReturningApiClient _client;

        public ApprenticeshipController(ResponseReturningApiClient client)
            => _client = client;

        [HttpPost("/apprenticeships")]
        public Task<IActionResult> CreateApprenticeship(CreateApprenticeshipFromRegistration request)
            => _client.Post("apprenticeships", request);

        [HttpGet("/apprenticeships/{id}")]
        public Task<IActionResult> GetApprenticeship(Guid id)
            => _client.Get($"/apprenticeships/{id}");

        [HttpGet("apprentices/{apprenticeId}/apprenticeships/{apprenticeshipId}")]
        public Task<IActionResult> GetApprenticeship(Guid apprenticeId, long apprenticeshipId)
            => _client.Get($"apprentices/{apprenticeId}/apprenticeships/{apprenticeshipId}");

        [HttpGet("apprentices/{apprenticeId}/apprenticeships/{apprenticeshipId}/revisions")]
        public Task<IActionResult> GetApprenticeshipRevisions(Guid apprenticeId, long apprenticeshipId)
            => _client.Get($"apprentices/{apprenticeId}/apprenticeships/{apprenticeshipId}/revisions");

        [HttpPatch("apprentices/{apprenticeId}/apprenticeships/{apprenticeshipId}")]
        public Task PatchApprenticeship(Guid apprenticeId, long apprenticeshipId, [FromBody] JsonPatchDocument<ApprenticeshipResponse> changes)
            => _client.Patch($"apprentices/{apprenticeId}/apprenticeships/{apprenticeshipId}", changes);

        [HttpPatch("apprentices/{apprenticeId}/apprenticeships/{apprenticeshipId}/revisions/{revisionId}/confirmations")]
        public Task ConfirmApprenticeship(Guid apprenticeId, long apprenticeshipId, long revisionId, [FromBody] Confirmations confirmations)
            => _client.Patch($"apprentices/{apprenticeId}/apprenticeships/{apprenticeshipId}/revisions/{revisionId}/confirmations", confirmations);
    }
}