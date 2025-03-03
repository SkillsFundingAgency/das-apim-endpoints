using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.AdminAan.Application.Locations.Queries.GetAddresses;
using SFA.DAS.AdminAan.Application.Locations.Queries.GetLocationsBySearch;

namespace SFA.DAS.AdminAan.Api.Controllers;

[Route("[controller]")]
public class LocationsController : ControllerBase
{
    private readonly IMediator _mediator;

    public LocationsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAddresses([FromQuery] string query)
    {
        var queryResponse = await _mediator.Send(new GetAddressesQuery(query));

        return Ok(queryResponse);
    }

    [HttpGet]
    [Route("search")]
    public async Task<IActionResult> GetLocationsBySearch([FromQuery] string query)
    {
        var queryResponse = await _mediator.Send(new GetLocationsBySearchQuery(query));

        return Ok(queryResponse);
    }
}