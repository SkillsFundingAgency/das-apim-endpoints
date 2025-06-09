using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.RoatpCourseManagement.Application.Locations.Queries.GetAddresses;
using System.Threading.Tasks;

namespace SFA.DAS.RoatpCourseManagement.Api.Controllers;

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
