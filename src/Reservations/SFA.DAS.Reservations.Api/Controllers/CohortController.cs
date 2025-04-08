using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.Reservations.Application.Providers.Queries.GetCohort;

namespace SFA.DAS.Reservations.Api.Controllers
{
    [ApiController]
    [Route("[controller]/")]
    public class CohortsController(IMediator mediator, ILogger<TransfersController> logger) : ControllerBase
    {
        [HttpGet]
        [Route("{cohortId}")]
        public async Task<IActionResult> Get(long cohortId)
        {
            try
            {
                var queryResult = await mediator.Send(new GetCohortQuery {CohortId = cohortId });
               
                if (queryResult?.Cohort == null)
                {
                    return NotFound();
                }

                return Ok(queryResult.Cohort);
            }
            catch (Exception e)
            {
                logger.LogError(e, $"Error attempting to get Cohort, Id: [{cohortId}]");
                return BadRequest();
            }
        }
    }
}
