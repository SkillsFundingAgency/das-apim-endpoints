using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.RoatpCourseManagement.Application.Locations.Queries.GetAddresses;

namespace SFA.DAS.RoatpCourseManagement.Api.Controllers;

[ApiController]
[Tags("Lookups")]
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
