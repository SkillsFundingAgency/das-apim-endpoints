using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.AdminAan.Application.Locations.Queries.GetAddresses;

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
}