using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApprenticeAan.Application.Calendars.Queries.GetCalendars;
using SFA.DAS.ApprenticeAan.Application.Models;

namespace SFA.DAS.ApprenticeAan.Api.Controllers;

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