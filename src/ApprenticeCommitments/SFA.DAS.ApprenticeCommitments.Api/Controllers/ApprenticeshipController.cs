using MediatR;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.ApprenticeCommitments.Apis.CommitmentsV2InnerApi;
using SFA.DAS.ApprenticeCommitments.Application.Commands;
using SFA.DAS.ApprenticeCommitments.Application.Services;
using SFA.DAS.SharedOuterApi.Exceptions;
using System;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeCommitments.Api.Controllers
{
    [ApiController]
    public class ApprenticeshipController : ControllerBase
    {
        private readonly ResponseReturningApiClient _client;
        private readonly IMediator _mediator;
        private readonly ILogger<ApprenticeshipController> _logger;

        public ApprenticeshipController(ResponseReturningApiClient client, IMediator mediator, ILogger<ApprenticeshipController> logger)
            => (_client, _mediator, _logger) = (client, mediator, logger);

        [HttpPost("/apprenticeships")]
        public async Task<IActionResult> CreateApprenticeship(CreateApprenticeshipFromRegistration.Command request)
        {
            _logger.LogInformation("POST CreateApprenticeship");
            _logger.LogInformation("Creating apprenticeship for apprentice {ApprenticeId} from registration {RegistrationId}",
                request.ApprenticeId, request.RegistrationId);

            try
            {
                await _mediator.Send(request);
                return Ok();
            }
            catch (ApiResponseException e)
            {
                _logger.LogError(e, "Creating apprenticeship for apprentice {ApprenticeId} from registration {RegistrationId}",
                    request.ApprenticeId, request.RegistrationId);

                return StatusCode((int)e.Status, e.Error);
            }
        }

        [HttpGet("/apprenticeships/{id}")]
        public Task<IActionResult> GetApprenticeship(Guid id)
            => _client.Get($"/apprenticeships/{id}");

        [HttpGet("/apprentices/{id}/apprenticeships")]
        public Task<IActionResult> ListApprenticeApprenticeships(Guid id)
            => _client.Get($"/apprentices/{id}/apprenticeships");

        [HttpGet("apprentices/{apprenticeId}/apprenticeships/{apprenticeshipId}")]
        public Task<IActionResult> GetApprenticeship(Guid apprenticeId, long apprenticeshipId)
            => _client.Get($"apprentices/{apprenticeId}/apprenticeships/{apprenticeshipId}");

        [HttpGet("apprentices/{apprenticeId}/apprenticeships/{apprenticeshipId}/confirmed/latest")]
        public Task<IActionResult> MyApprenticeship(Guid apprenticeId, long apprenticeshipId)
            => _client.Get($"apprentices/{apprenticeId}/apprenticeships/{apprenticeshipId}/confirmed/latest");

        [HttpGet("apprentices/{apprenticeId}/apprenticeships/{apprenticeshipId}/revisions")]
        public Task<IActionResult> GetApprenticeshipRevisions(Guid apprenticeId, long apprenticeshipId)
            => _client.Get($"apprentices/{apprenticeId}/apprenticeships/{apprenticeshipId}/revisions");

        [HttpPatch("apprentices/{apprenticeId}/apprenticeships/{apprenticeshipId}")]
        public Task PatchApprenticeship(Guid apprenticeId, long apprenticeshipId, [FromBody] JsonPatchDocument<ApprenticeshipResponse> changes)
            => _client.Patch($"apprentices/{apprenticeId}/apprenticeships/{apprenticeshipId}", changes);
    }
}