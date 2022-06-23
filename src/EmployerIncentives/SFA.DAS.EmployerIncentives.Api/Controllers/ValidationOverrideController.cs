using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.EmployerIncentives.Application.Commands.ValidationOverride;
using SFA.DAS.EmployerIncentives.InnerApi.Requests;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerIncentives.Api.Controllers
{
    [ApiController]
    public class ValidationOverrideController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ValidationOverrideController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Route("validation-overrides")]        
        public async Task<IActionResult> Add([FromBody] ValidationOverrideRequest request)
        {
            await _mediator.Send(new ValidationOverrideCommand(request), CancellationToken.None);

            return Accepted();
        }
    }
}
