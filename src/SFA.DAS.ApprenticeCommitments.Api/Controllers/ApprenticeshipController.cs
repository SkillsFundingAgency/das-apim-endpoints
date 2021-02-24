using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApprenticeCommitments.Application.Commands.ChangeEmailAddress;
using SFA.DAS.ApprenticeCommitments.Application.Commands.CreateApprenticeship;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeCommitments.Api.Controllers
{
    [ApiController]
    public class ApprenticeshipController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ApprenticeshipController(IMediator mediator) => _mediator = mediator;

        [HttpPost]
        [Route("/apprenticeships")]
        public async Task<IActionResult> AddApprenticeship(CreateApprenticeshipCommand request)
        {
            await _mediator.Send(request);
            return Accepted();
        }

        [HttpPost("/apprentices/{apprenticeId}/email")]
        public async Task<IActionResult> ChangeApprenticeEmailAddress(long apprenticeId, ChangeEmailAddressCommand request)
        {
            request.ApprenticeId = apprenticeId;
            await _mediator.Send(request);
            return Ok();
        }
    }
}