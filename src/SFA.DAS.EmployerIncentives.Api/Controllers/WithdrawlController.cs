using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.EmployerIncentives.Application.Commands.AddJobRequest;
using SFA.DAS.EmployerIncentives.Application.Commands.WithdrawApplication;
using SFA.DAS.EmployerIncentives.InnerApi.Requests;

namespace SFA.DAS.EmployerIncentives.Api.Controllers
{
    [ApiController]
    public class WithdrawlController : ControllerBase
    {
        private readonly IMediator _mediator;

        public WithdrawlController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPut]
        [Route("/withdrawls")]
        public async Task<IActionResult> Withdraw([FromBody] WithdrawRequest request)
        {
            await _mediator.Send(new WithdrawCommand(request), CancellationToken.None);

            return Accepted();
        }
    }
}
