using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApprenticeCommitments.Application.Commands.VerifyIdentityRegistration;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeCommitments.Api.Controllers
{
    [ApiController]
    public class RegistrationController : ControllerBase
    {
        private readonly IMediator _mediator;

        public RegistrationController(IMediator mediator) => _mediator = mediator;

        [HttpPost]
        [Route("/registrations")]
        public async Task<IActionResult> VerifyRegistration(VerifyIdentityRegistrationCommand request)
        {
            await _mediator.Send(request);
            return Ok();
        }

    }
}