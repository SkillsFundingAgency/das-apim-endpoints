using System;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using SFA.DAS.ApprenticeCommitments.Apis.InnerApi;
using SFA.DAS.ApprenticeCommitments.Application.Commands.SendInvitationReminders;
using SFA.DAS.ApprenticeCommitments.Application.Commands.VerifyIdentityRegistration;
using SFA.DAS.ApprenticeCommitments.Application.Queries.Registration;
using SFA.DAS.ApprenticeCommitments.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

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
            var result = await _mediator.Send(new RegistrationQuery { ApprenticeshipId = apprenticeId });
            if (result == null)
            {
                return NotFound();
            }
            return new OkObjectResult(result);
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

        [HttpPost]
        [Route("/registrations/reminders")]
        public async Task<IActionResult> SendReminders(SendInvitationRemindersCommand request)
        {
            await _mediator.Send(request);
            return Ok();
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