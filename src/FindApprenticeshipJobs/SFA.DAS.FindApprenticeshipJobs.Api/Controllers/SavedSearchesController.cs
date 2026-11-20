using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.FindApprenticeshipJobs.Application.Queries.SavedSearch.GetSavedSearches;
using System.Net;
using SFA.DAS.FindApprenticeshipJobs.Api.Models;
using SFA.DAS.FindApprenticeshipJobs.Application.Queries.SavedSearch.GetSavedSearchVacancies;
using SFA.DAS.FindApprenticeshipJobs.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.Interfaces;

namespace SFA.DAS.FindApprenticeshipJobs.Api.Controllers;

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
        CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Get Candidates for Saved Searches invoked");

        try
        {
            var result = await mediator.Send(new GetSavedSearchesQuery(lastRunDateTime, pageNumber, pageSize), cancellationToken);
            return Ok(result);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error invoking Get Candidates for Saved Searches");
            return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
        }
    }
        
    [HttpPost]
    [Route("GetSavedSearchResult")]
    public async Task<IActionResult> GetSavedSearchResult(
        [FromServices] IFindApprenticeshipApiClient<FindApprenticeshipApiConfiguration> faaInnerClient,
        [FromBody] GetCandidateSavedVacanciesRequest request,
        CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Get Saved Searches invoked");
        try
        {
            var result = await mediator.Send(new GetSavedSearchVacanciesQuery(
                    request.ApprenticeshipSearchResultsSortOrder,
                    request.Id,
                    request.UserId,
                    request.Distance,
                    request.SearchTerm,
                    request.Location,
                    request.DisabilityConfident,
                    request.ExcludeNational,
                    request.Longitude,
                    request.Latitude,
                    request.SelectedLevelIds,
                    request.SelectedRouteIds,
                    request.UnSubscribeToken,
                    request.LastRunDateFilter,
                    request.PageNumber,
                    request.PageSize,
                    request.SelectedApprenticeshipTypes),
                    cancellationToken);
            
            if (result is not null)
            {
                return Ok(result);
            }
            
            // user is either dormant, deleted
            await faaInnerClient.Delete(new DeleteSavedSearchesRequest([request.Id]));
            return NotFound();
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

    [HttpPost]
    [Route("delete-many")]
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    public async Task<IActionResult> DeleteMany(
        [FromBody] List<Guid> ids,
        [FromServices] IFindApprenticeshipApiClient<FindApprenticeshipApiConfiguration> faaInnerClient,
        CancellationToken cancellationToken)
    {
        await faaInnerClient.Delete(new DeleteSavedSearchesRequest(ids));
        return NoContent();
    }
}