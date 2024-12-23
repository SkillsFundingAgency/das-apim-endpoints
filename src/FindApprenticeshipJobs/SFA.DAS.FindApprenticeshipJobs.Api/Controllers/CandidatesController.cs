using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.FindApprenticeshipJobs.Application.Queries.SavedSearch.GetCandidatesByActivity;
using System.Net;
using SFA.DAS.FindApprenticeshipJobs.Application.Commands.Candidates;
using SFA.DAS.FindApprenticeshipJobs.Domain.Models;
using SFA.DAS.FindApprenticeshipJobs.Api.Models;

namespace SFA.DAS.FindApprenticeshipJobs.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CandidatesController(
        IMediator mediator,
        ILogger<CandidatesController> logger) : ControllerBase
    {
        [HttpGet]
        [Route("GetCandidatesByActivity")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> GetCandidatesByActivity(
            [FromQuery] DateTime cutOffDateTime,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10,
            CancellationToken cancellationToken = default)
        {
            logger.LogInformation("Get Candidates by activity invoked");

            try
            {
                var result = await mediator.Send(new GetCandidateByActivityQuery(cutOffDateTime, pageNumber, pageSize),
                    cancellationToken);
                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error invoking Get Candidates By Activity");
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }

        [HttpPost]
        [Route("{govIdentifier}/status")]
        public async Task<IActionResult> UpdateStatus(
            [FromRoute] string govIdentifier,
            [FromBody] CandidateUpdateStatusRequest request)
        {
            try
            {
                await mediator.Send(new UpdateCandidateStatusCommand
                {
                    GovUkIdentifier = govIdentifier,
                    Email = request.Email,
                    Status = request.Status
                });

                return NoContent();
            }
            catch (Exception e)
            {
                logger.LogError(e, "Error attempting to update candidate status");
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }
    }
}