using System;
using System.Net;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.FindAnApprenticeship.Api.Models.SavedSearches;
using SFA.DAS.FindAnApprenticeship.Application.Commands.SaveSearch;
using SFA.DAS.FindAnApprenticeship.Application.Queries.SavedSearches;

namespace SFA.DAS.FindAnApprenticeship.Api.Controllers;

[ApiController]
[Route("saved-searches/")]
public class SavedSearchesController(IMediator mediator, ILogger<SavedSearchesController> logger) : ControllerBase
{
    [HttpGet]
    [Route("{id:guid}/unsubscribe")]
    public async Task<IActionResult> GetUnsubscribeSavedSearch([FromRoute] Guid id)
    {
        try
        {
            var result = await mediator.Send(new GetUnsubscribeSavedSearchQuery(id)
            {
                SavedSearchId = id
            });

            return Ok(result);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Get Unsubscribe Saved Search : An error occurred");
            return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
        }
    }

    [HttpPost]
    [Route("unsubscribe")]
    public async Task<IActionResult> PostUnsubscribeSavedSearch([FromBody]PostUnsubscribeSavedSearchApiRequest request)
    {
        try
        {
            await mediator.Send(new UnsubscribeSavedSearchCommand(request.SavedSearchId));
            return Ok();
        }
        catch (Exception e)
        {
            logger.LogError(e, "Post Unsubscribe Saved Search : An error occurred");
            return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
        }
    }
}