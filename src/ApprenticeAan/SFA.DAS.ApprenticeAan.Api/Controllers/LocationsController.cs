using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApprenticeAan.Application.Locations.Queries.GetAddresses;
using SFA.DAS.ApprenticeAan.Application.Locations.Queries.GetPostcodes;

namespace SFA.DAS.ApprenticeAan.Api.Controllers;

[ApiController]
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
    [Route("{postcode}")]
    public async Task<IActionResult> GetCoordinates([FromRoute] string postcode)
    {
        var queryResponse = await _mediator.Send(new GetPostcodeQuery(postcode));

        if (queryResponse == null) return NotFound();

        return Ok(queryResponse);
    }
}