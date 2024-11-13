using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.FindApprenticeshipJobs.Application.Queries.SavedSearch.GetSavedSearches;
using SFA.DAS.FindApprenticeshipJobs.Domain.Models;
using System.Net;
using SFA.DAS.FindApprenticeshipJobs.Api.Models;

namespace SFA.DAS.FindApprenticeshipJobs.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SavedSearchesController(
        IMediator mediator,
        ILogger<SavedSearchesController> logger) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> Get(
            [FromQuery] DateTime lastRunDateTime, 
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] int maxApprenticeshipSearchResultCount = 5,
            [FromQuery] VacancySort sortOrder = VacancySort.AgeDesc, 
            CancellationToken cancellationToken = default)
        {
            logger.LogInformation("Get Saved Searches invoked");

            try
            {
                var result = await mediator.Send(new GetSavedSearchesQuery(lastRunDateTime,
                        pageNumber,
                        pageSize,
                        maxApprenticeshipSearchResultCount,
                        sortOrder),
                    cancellationToken);
                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error invoking Get Saved Searches");
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }

        [HttpPost]
        [Route("sendNotification")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> SendNotification(
            [FromBody] SavedSearchApiRequest request,
            CancellationToken cancellationToken = default)
        {
            logger.LogInformation("Post send saved search notification invoked");

            try
            {
                await mediator.Send(new SavedSearchApiRequest().MapToCommand(request), cancellationToken);

                return NoContent();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error invoking Post Send saved Search notification");
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }
    }
}
