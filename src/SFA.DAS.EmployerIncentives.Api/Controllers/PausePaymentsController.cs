using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.EmployerIncentives.InnerApi.Requests;
using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.EmployerIncentives.Application.Commands.PausePayments;

namespace SFA.DAS.EmployerIncentives.Api.Controllers
{
    [ApiController]
    public class PausePaymentsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PausePaymentsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Route("/pause-payments")]
        public async Task<IActionResult> PausePayments([FromBody] PausePaymentsRequest request)
        {
            await _mediator.Send(new PausePaymentsCommand(request), CancellationToken.None);

            return Ok();
        }
    }
}
