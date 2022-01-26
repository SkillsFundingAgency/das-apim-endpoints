using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.EmploymentCheck.Api.Models;
using SFA.DAS.EmploymentCheck.Application.Commands.RegisterCheck;
using SFA.DAS.SharedOuterApi.Infrastructure;
using System.Threading.Tasks;

namespace SFA.DAS.EmploymentCheck.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]/")]
    public class EmploymentCheckController : ControllerBase
    {
        private readonly IMediator _mediator;

        public EmploymentCheckController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Route("RegisterCheck")]
        public async Task<IActionResult> RegisterCheck(RegisterCheckRequest request)
        {
            try
            {
                var response = await _mediator.Send(new RegisterCheckCommand
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