using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApprenticeCommitments.Apprenticeship.Commands;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeCommitments.Api.Controllers
{
    [ApiController]
    public class ApprenticeshipController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ApprenticeshipController(IMediator mediator) => _mediator = mediator;

        [HttpPost]
        [Route("/apprenticeship")]
        public async Task<IActionResult> AddApprenticeship(CreateApprenticeshipCommand request)
        {
            await _mediator.Send(request);
            return Ok();
        }
    }
}