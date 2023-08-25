using System.Globalization;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.EmployerAan.Application.Calendars.Queries.GetCalendars;

namespace SFA.DAS.EmployerAan.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class CalendarsController : ControllerBase
{
    private readonly IMediator _mediator;

    public CalendarsController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    [Produces("application/json")]
    [ProducesResponseType(typeof(IEnumerable<Calendar>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetCalendars(CancellationToken cancellationToken)
    {
        return Ok(await _mediator.Send(new GetCalendarsQuery(), cancellationToken));
    }
}
