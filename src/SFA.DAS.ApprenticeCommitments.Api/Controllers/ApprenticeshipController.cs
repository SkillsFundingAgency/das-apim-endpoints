using MediatR;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApprenticeCommitments.Apis.CommitmentsV2InnerApi;
using SFA.DAS.ApprenticeCommitments.Application.Commands;
using SFA.DAS.ApprenticeCommitments.Application.Services;
using System;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeCommitments.Api.Controllers
{
    [ApiController]
    public class ApprenticeshipController : ControllerBase
    {
        private readonly ResponseReturningApiClient _client;
        private readonly IMediator _mediator;

        public ApprenticeshipController(ResponseReturningApiClient client, IMediator mediator)
            => (_client, _mediator) = (client, mediator);

        [HttpPost("/apprenticeships")]
        public Task CreateApprenticeship(CreateApprenticeshipFromRegistration.Command request)
            => _mediator.Send(request);

        [HttpGet("/apprenticeships/{id}")]
        public Task<IActionResult> GetApprenticeship(Guid id)
            => _client.Get($"/apprenticeships/{id}");

        [HttpGet("/apprentices/{id}/apprenticeships")]
        public Task<IActionResult> ListApprenticeApprenticeships(Guid id)
            => _client.Get($"/apprentices/{id}/apprenticeships");

        [HttpGet("apprentices/{apprenticeId}/apprenticeships/{apprenticeshipId}")]
        public Task<IActionResult> GetApprenticeship(Guid apprenticeId, long apprenticeshipId)
            => _client.Get($"apprentices/{apprenticeId}/apprenticeships/{apprenticeshipId}");

        [HttpGet("apprentices/{apprenticeId}/apprenticeships/{apprenticeshipId}/revisions")]
        public Task<IActionResult> GetApprenticeshipRevisions(Guid apprenticeId, long apprenticeshipId)
            => _client.Get($"apprentices/{apprenticeId}/apprenticeships/{apprenticeshipId}/revisions");

        [HttpPatch("apprentices/{apprenticeId}/apprenticeships/{apprenticeshipId}")]
        public Task PatchApprenticeship(Guid apprenticeId, long apprenticeshipId, [FromBody] JsonPatchDocument<ApprenticeshipResponse> changes)
            => _client.Patch($"apprentices/{apprenticeId}/apprenticeships/{apprenticeshipId}", changes);
    }
}