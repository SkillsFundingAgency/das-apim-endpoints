using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.EmployerAan.Application.Locations.Queries.GetLocationsBySearch;

namespace SFA.DAS.EmployerAan.Api.Controllers;

[Route("[controller]")]
public class LocationsController : ControllerBase
{
    private readonly IMediator _mediator;

    public LocationsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [Route("search")]
    public async Task<IActionResult> GetLocationsBySearch([FromQuery] string query)
    {
        var queryResponse = await _mediator.Send(new GetLocationsBySearchQuery(query));

        return Ok(queryResponse);
    }
}