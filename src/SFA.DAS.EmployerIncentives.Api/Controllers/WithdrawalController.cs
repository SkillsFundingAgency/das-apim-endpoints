using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.EmployerIncentives.Application.Commands.WithdrawApplication;
using SFA.DAS.EmployerIncentives.InnerApi.Requests;
using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.EmployerIncentives.Application.Commands.ReinstateApplication;

namespace SFA.DAS.EmployerIncentives.Api.Controllers
{
    [ApiController]
    public class WithdrawalController : ControllerBase
    {
        private readonly IMediator _mediator;

        public WithdrawalController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Route("/withdrawals")]
        public async Task<IActionResult> Withdraw([FromBody] WithdrawRequest request)
        {
            await _mediator.Send(new WithdrawCommand(request), CancellationToken.None);

            return Accepted();
        }

        [HttpPost]
        [Route("/withdrawal-reinstatements")]
        public async Task<IActionResult> ReinstateApplication([FromBody] ReinstateApplicationRequest request)
        {
            await _mediator.Send(new ReinstateApplicationCommand(request), CancellationToken.None);

            return Accepted();
        }
    }
}
