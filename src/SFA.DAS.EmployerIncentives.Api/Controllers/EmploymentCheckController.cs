using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.EmployerIncentives.Api.Models;
using SFA.DAS.EmployerIncentives.Application.Commands.EmploymentCheck;
using System.Threading.Tasks;
using SFA.DAS.EmployerIncentives.Application.Commands.RegisterEmploymentCheck;
using SFA.DAS.SharedOuterApi.Infrastructure;

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

        [HttpPost]
        [Route("/employmentchecks")]
        public async Task<IActionResult> RegisterCheck(RegisterCheckRequest request)
        {
            try
            {
                var response = await _mediator.Send(new RegisterEmploymentCheckCommand
                {
                    CorrelationId = request.CorrelationId,
                    CheckType = request.CheckType,
                    Uln = request.Uln,
                    ApprenticeshipAccountId = request.ApprenticeshipAccountId,
                    ApprenticeshipId = request.ApprenticeshipId,
                    MinDate = request.MinDate,
                    MaxDate = request.MaxDate
                });

                return Ok(response);
            }
            catch (HttpRequestContentException ex)
            {
                return StatusCode((int)ex.StatusCode, ex.ErrorContent);
            }
        }
    }
}