using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using SFA.DAS.EmployerIncentives.Api.Models;
using SFA.DAS.EmployerIncentives.Application.Commands.RecalculateEarnings;
using SFA.DAS.EmployerIncentives.Application.Queries.GetApprenticeshipIncentives;
using SFA.DAS.EmployerIncentives.InnerApi.Requests.RecalculateEarnings;
using SFA.DAS.SharedOuterApi.Infrastructure;

namespace SFA.DAS.EmployerIncentives.Api.Controllers
{
    [ApiController]
    public class ApprenticeshipIncentiveController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ApprenticeshipIncentiveController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Route("/accounts/{accountId}/legalentities/{accountLegalEntityId}/apprenticeshipIncentives")]
        public async Task<IActionResult> GetApprenticeshipIncentives(long accountId, long accountLegalEntityId)
        {
            var queryResult = await _mediator.Send(new GetApprenticeshipIncentivesQuery
            {
                AccountId = accountId,
                AccountLegalEntityId = accountLegalEntityId
            });

            var response = queryResult.ApprenticeshipIncentives.Select(c => (ApprenticeshipIncentiveDto) c).ToArray();

            return Ok(response);
        }

        [HttpPost]
        [Route("/earningsRecalculations")]
        public async Task<IActionResult> RecalculateEarnings([FromBody] RecalculateEarningsRequest request) 
        {
            try
            {
                await _mediator.Send(new RecalculateEarningsCommand(request));

                return NoContent();
            }
            catch (HttpRequestContentException requestException) 
            {
                return BadRequest(requestException.ErrorContent);
            }
        }
    }
}