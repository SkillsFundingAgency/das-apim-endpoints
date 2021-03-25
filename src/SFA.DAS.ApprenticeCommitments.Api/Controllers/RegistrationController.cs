using System;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using SFA.DAS.ApprenticeCommitments.Application.Commands.SendInvitationReminders;
using SFA.DAS.ApprenticeCommitments.Application.Commands.VerifyIdentityRegistration;
using SFA.DAS.ApprenticeCommitments.Application.Queries.Registration;

namespace SFA.DAS.ApprenticeCommitments.Api.Controllers
{
    [ApiController]
    public class RegistrationController : ControllerBase
    {
        private readonly IMediator _mediator;

        public RegistrationController(IMediator mediator) => _mediator = mediator;

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

        [HttpPost]
        [Route("/registrations/reminders")]
        public async Task<IActionResult> SendReminders(SendInvitationRemindersCommand request)
        {
            await _mediator.Send(request);
            return Ok();
        }


    }
}