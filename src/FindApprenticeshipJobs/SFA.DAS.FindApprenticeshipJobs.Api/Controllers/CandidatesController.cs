using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.FindApprenticeshipJobs.Application.Queries.SavedSearch.GetCandidatesByActivity;
using System.Net;

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
        public async Task<IActionResult> GetCandidatesByActivity(
            [FromQuery] DateTime cutOffDateTime,
            CancellationToken cancellationToken = default)
        {
            logger.LogInformation("Get Candidates By Activity invoked");

            try
            {
                var result = await mediator.Send(new GetCandidateByActivityQuery(cutOffDateTime),
                    cancellationToken);
                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error invoking Get Candidates By Activity");
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }
    }
}