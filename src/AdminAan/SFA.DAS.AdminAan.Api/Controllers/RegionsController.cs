using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.AdminAan.Application.Regions.Queries.GetRegions;

namespace SFA.DAS.AdminAan.Api.Controllers;

[Route("[controller]")]
[ApiController]
public class RegionsController : ControllerBase
{
    private readonly IMediator _mediator;

    public RegionsController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    [Produces("application/json")]
    [ProducesResponseType(typeof(GetRegionsQueryResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetRegions(CancellationToken cancellationToken)
    {
        return Ok(await _mediator.Send(new GetRegionsQuery(), cancellationToken));
    }
}
