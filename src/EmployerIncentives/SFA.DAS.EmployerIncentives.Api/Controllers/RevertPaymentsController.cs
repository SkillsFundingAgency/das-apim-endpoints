using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SFA.DAS.EmployerIncentives.Api.Models;
using SFA.DAS.EmployerIncentives.Application.Commands.RevertPayments;
using SFA.DAS.EmployerIncentives.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Infrastructure;

namespace SFA.DAS.EmployerIncentives.Api.Controllers
{
    public class RevertPaymentsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public RevertPaymentsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Route("revert-payments")]
        public async Task<IActionResult> RevertPayments([FromBody] RevertPaymentsRequest request)
        {
            try
            {
                await _mediator.Send(new RevertPaymentsCommand(request), CancellationToken.None);

                return Ok();
            }
            catch (HttpRequestContentException e)
            {
                return new BadRequestObjectResult(e.ErrorContent);
            }
        }
    }
}
