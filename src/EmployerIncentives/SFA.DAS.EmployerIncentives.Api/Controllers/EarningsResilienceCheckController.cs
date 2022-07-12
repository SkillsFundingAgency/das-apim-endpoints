using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.EmployerIncentives.Application.Commands.EarningsResilienceCheck;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerIncentives.Api.Controllers
{
    [ApiController]
    public class EarningsResilienceCheckController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<EarningsResilienceCheckController> _logger;

        public EarningsResilienceCheckController(IMediator mediator, ILogger<EarningsResilienceCheckController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpPost]
        [Route("/earnings-resilience-check")]
        public async Task<IActionResult> EarningsResilienceCheck()
        {
            await _mediator.Send(new EarningsResilienceCheckCommand());

            return new OkResult();
        }
    }
}
