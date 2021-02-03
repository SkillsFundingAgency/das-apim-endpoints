using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using SFA.DAS.ApprenticeCommitments.Application.Commands.CreateApprenticeship;

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
    }
}