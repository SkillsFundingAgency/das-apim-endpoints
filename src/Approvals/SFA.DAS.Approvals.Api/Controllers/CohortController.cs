using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.Approvals.Application.Cohorts.Queries;
using System;
using System.Threading.Tasks;
using SFA.DAS.Approvals.Api.Models.Cohorts;
using SFA.DAS.Approvals.Application.Cohorts.Queries.GetCohortDetails;

namespace SFA.DAS.Approvals.Api.Controllers
{
    [ApiController]
    public class CohortController : ControllerBase
    {
        private readonly ILogger<DraftApprenticeshipController> _logger;
        private readonly IMediator _mediator;

        public CohortController(ILogger<DraftApprenticeshipController> logger,
            IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpGet]
        [Route("[controller]/{cohortId}")]
        public async Task<IActionResult> Get(long cohortId)
        {
            try
            {
                var result = await _mediator.Send(new GetCohortQuery(cohortId));
                if (result == null)
                {
                    return NotFound();
                }
                return Ok(result);
            }
            catch (Exception e)
            {
                _logger.LogError($"Error getting cohort with {cohortId}", e);
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("employer/{accountId}/unapproved/{cohortId}")]
        [Route("provider/{providerId}/unapproved/{cohortId}")]
        public async Task<IActionResult> GetCohortDetails(long cohortId)
        {
            try
            {
                var result = await _mediator.Send(new GetCohortDetailsQuery { CohortId = cohortId });

                if (result == null)
                {
                    return NotFound();
                }

                return Ok((GetCohortDetailsResponse)result);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error in Get Cohort Details - cohort id {cohortId}");
                return BadRequest();
            }
        }
    }
}
