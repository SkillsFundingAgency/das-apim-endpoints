using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApprenticeCommitments.Apis.CommitmentsV2InnerApi;
using SFA.DAS.ApprenticeCommitments.Application.Services;
using System;
using System.Threading.Tasks;
using SFA.DAS.ApprenticeCommitments.Apis.InnerApi;

namespace SFA.DAS.ApprenticeCommitments.Api.Controllers
{
    [ApiController]
    public class RevisionController : ControllerBase
    {
        private readonly ResponseReturningApiClient _client;

        public RevisionController(ResponseReturningApiClient client)
        {
            _client = client;
        }

        [HttpPatch("apprentices/{apprenticeId}/apprenticeships/{apprenticeshipId}/revisions/{revisionId}")]
        public Task PatchApprenticeship(Guid apprenticeId, long apprenticeshipId, long revisionId, [FromBody] JsonPatchDocument<RevisionPatch> changes)
            => _client.Patch($"apprentices/{apprenticeId}/apprenticeships/{apprenticeshipId}/revisions/{revisionId}", changes);

        [HttpPatch("apprentices/{apprenticeId}/apprenticeships/{apprenticeshipId}/revisions/{revisionId}/confirmations")]
        public Task ConfirmApprenticeship(Guid apprenticeId, long apprenticeshipId, long revisionId, [FromBody] Confirmations confirmations)
            => _client.Patch($"apprentices/{apprenticeId}/apprenticeships/{apprenticeshipId}/revisions/{revisionId}/confirmations", confirmations);

        [HttpGet("apprentices/{apprenticeId}/apprenticeships/{apprenticeshipId}/revisions/{revisionId}")]
        public Task<IActionResult> GetApprenticeshipRevision(Guid apprenticeId, long apprenticeshipId, long revisionId)
            => _client.Get($"apprentices/{apprenticeId}/apprenticeships/{apprenticeshipId}/revisions/{revisionId}");
    }
}