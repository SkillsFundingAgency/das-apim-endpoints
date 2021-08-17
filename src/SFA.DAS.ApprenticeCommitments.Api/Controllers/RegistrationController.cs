using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApprenticeCommitments.Apis.InnerApi;
using SFA.DAS.ApprenticeCommitments.Application.Commands.VerifyIdentityRegistration;
using SFA.DAS.ApprenticeCommitments.Application.Queries.Registration;
using SFA.DAS.ApprenticeCommitments.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using System;
using System.Threading.Tasks;
using SFA.DAS.SharedOuterApi.Infrastructure;

namespace SFA.DAS.ApprenticeCommitments.Api.Controllers
{
    [ApiController]
    public class RegistrationController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IInternalApiClient<ApprenticeCommitmentsConfiguration> _client;

        public RegistrationController(IMediator mediator, IInternalApiClient<ApprenticeCommitmentsConfiguration> client)
        {
            _mediator = mediator;
            _client = client;
        }

        [HttpGet]
        [Route("/registrations/{apprenticeId}")]
        public async Task<IActionResult> Get(Guid apprenticeId)
        {
            var response = await _client.GetWithResponseCode<RegistrationResponse>(
                new GetRegistrationDetailsRequest(apprenticeId));

            if (response.Body != null)
                return StatusCode((int)response.StatusCode, response.Body);
            else
                return StatusCode((int)response.StatusCode, response.ErrorContent);
        }

        [HttpPost]
        [Route("/registrations")]
        public async Task<IActionResult> VerifyRegistration(VerifyIdentityRegistrationCommand request)
        {
            await _mediator.Send(request);
            return Ok();
        }

        [HttpGet]
        [Route("/registrations/reminders")]
        public async Task<IActionResult> SendReminders([FromQuery] DateTime invitationCutOffTime)
        {
            return await new GetRequest<RegistrationsWrapper>($"/registrations/reminders?invitationCutOffTime={invitationCutOffTime}").Get(_client);
        }

        [HttpPost("registrations/{apprenticeId}/reminder")]
        public async Task<IActionResult> RegistrationReminderSent(Guid apprenticeId, [FromBody] InvitationReminderSentRequest request)
        {
            return await new PostRequest($"registrations/{apprenticeId}/reminder").Post(_client, request);
        }


        [HttpPost("/registrations/{apprenticeId}/firstseen")]
        public async Task<IActionResult> RegistrationFirstSeen(Guid apprenticeId,
            [FromBody] RegistrationFirstSeenRequestData request)
        {
            await _client.Post(new RegistrationFirstSeenRequest(apprenticeId, request));
            return Accepted();
        }
    }
}