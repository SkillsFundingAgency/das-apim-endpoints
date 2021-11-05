using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApprenticeCommitments.Apis.InnerApi;
using SFA.DAS.ApprenticeCommitments.Application.Commands.ChangeRegistration;
using SFA.DAS.ApprenticeCommitments.Application.Commands.CreateRegistration;
using SFA.DAS.ApprenticeCommitments.Application.Services;
using System;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeCommitments.Api.Controllers
{
    [ApiController]
    public class RegistrationController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ResponseReturningApiClient _client;

        public RegistrationController(IMediator mediator, ResponseReturningApiClient client)
        {
            _mediator = mediator;
            _client = client;
        }

        [HttpPost("/registrations")]
        public async Task<ActionResult<CreateRegistrationResponse>> CreateRegistrationFromApproval(CreateRegistrationCommand request)
            => await _mediator.Send(request);

        [HttpPut]
        [Route("/registrations")]
        public async Task UpdateRegistrationFromApproval(ChangeRegistrationCommand request)
            => await _mediator.Send(request);

        [HttpGet]
        [Route("/registrations/{registrationId}")]
        public Task<IActionResult> Get(Guid registrationId)
            => _client.Get($"registrations/{registrationId}");

        [HttpGet]
        [Route("/registrations/reminders")]
        public Task<IActionResult> GetRemindersToSend([FromQuery] DateTime invitationCutOffTime)
            => _client.Get($"registrations/reminders?invitationCutOffTime={invitationCutOffTime}");

        [HttpPost("/registrations/{registrationId}/reminder")]
        public Task<IActionResult> RegistrationReminderSent(Guid registrationId, [FromBody] InvitationReminderSentRequest request)
            => _client.Post($"registrations/{registrationId}/reminder", request);

        [HttpPost("/registrations/{registrationId}/firstseen")]
        public Task<IActionResult> RegistrationFirstSeen(Guid registrationId, [FromBody] RegistrationFirstSeenRequestData request)
            => _client.Post($"registrations/{registrationId}/firstseen", request);

        [HttpPost("/registrations/stopped")]
        public Task<IActionResult> StopRegistration([FromBody] StopRegistrationCommand request)
            => _client.Post("registrations/stopped", request);

    }
}