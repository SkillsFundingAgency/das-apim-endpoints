using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.FindApprenticeshipJobs.Application.Queries.SavedSearch.GetSavedSearches;
using SFA.DAS.FindApprenticeshipJobs.Domain.Models;
using System.Net;

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
            [FromQuery] int pageNumber,
            [FromQuery] int pageSize,
            [FromQuery] int maxApprenticeshipSearchResultCount = 10,
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
    }
}
