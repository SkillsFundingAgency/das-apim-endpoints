using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.AdminAan.Application.Calendars.Queries.GetCalendars;
using System.Globalization;

namespace SFA.DAS.AdminAan.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class CalendarsController : Controller
{
    private readonly IMediator _mediator;

    public CalendarsController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    [Produces("application/json")]
    [ProducesResponseType(typeof(IEnumerable<Calendar>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GeCalendars(CancellationToken cancellationToken)
    {
        return Ok(await _mediator.Send(new GetCalendarsQuery(), cancellationToken));
    }
}