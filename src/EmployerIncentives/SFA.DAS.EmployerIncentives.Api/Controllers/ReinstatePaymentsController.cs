using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.EmployerIncentives.Application.Commands.ReinstatePayments;
using SFA.DAS.EmployerIncentives.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Infrastructure;

namespace SFA.DAS.EmployerIncentives.Api.Controllers
{
    public class ReinstatePaymentsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ReinstatePaymentsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Route("reinstate-payments")]
        public async Task<IActionResult> ReinstatePayments([FromBody] ReinstatePaymentsRequest request)
        {
            try
            {
                await _mediator.Send(new ReinstatePaymentsCommand(request), CancellationToken.None);

                return Ok();
            }
            catch (HttpRequestContentException e)
            {
                return new BadRequestObjectResult(e.ErrorContent);
            }
        }
    }
}
