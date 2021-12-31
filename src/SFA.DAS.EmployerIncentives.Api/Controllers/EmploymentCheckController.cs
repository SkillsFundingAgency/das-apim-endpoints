using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.EmployerIncentives.Api.Models;
using SFA.DAS.EmployerIncentives.Application.Commands.EmploymentCheck;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerIncentives.Api.Controllers
{
    [ApiController]
    public class EmploymentCheckController : ControllerBase
    {
        private readonly IMediator _mediator;

        public EmploymentCheckController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPut]
        [Route("/employmentchecks/{correlationId}")]
        public async Task<IActionResult> UpdateCheck(UpdateEmploymentCheckRequest request)
        {
            await _mediator.Send(new EmploymentCheckCommand(request.CorrelationId, request.Result, request.DateChecked));

            return new OkResult();
        }
    }
}