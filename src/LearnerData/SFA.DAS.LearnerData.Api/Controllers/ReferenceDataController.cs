using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.LearnerData.Application.GetProviderRelationships;
using SFA.DAS.LearnerData.Extensions;

namespace SFA.DAS.LearnerData.Api.Controllers;

[Route("reference-data")]
[ApiController]
public class ReferenceDataController(
IMediator mediator,
ILogger<ReferenceDataController> logger) : ControllerBase
{
    [HttpGet("providers/{ukprn}")]
    public async Task<IActionResult> GetProviderRelationshipDetails([FromRoute] int ukprn)
    {
        var query = new GetProviderRelationshipQuery()
        {
            Ukprn = ukprn
        };

        logger.LogInformation("Started handling provider relationshipquery");
        var response = await mediator.Send(query);
        logger.LogInformation("finished handling provider relationshipquery");
        logger.LogInformation($" is response null : {response is null}");

        if (response is null) return NotFound();

        return Ok(response);
    }

    [HttpGet("providers")]
    public async Task<IActionResult> GetAllProviderRelationshipDetails([FromQuery] int page = 1, [FromQuery] int? pagesize = 20)
    {
        const int MAX_PAGE_SIZE = 100;

        if (pagesize < 1 || pagesize > MAX_PAGE_SIZE)
            return BadRequest();

        var query = new GetAllProviderRelationshipQuery()
        {
            Page = page,
            PageSize = pagesize
        };

        var response = await mediator.Send(query);

        HttpContext.SetPageLinksInResponseHeaders(query, response);

        return Ok(response);
    }
}