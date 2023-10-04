using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.AdminAan.Api.Models;
using SFA.DAS.AdminAan.Application.CalendarEvents.Commands.Create;
using SFA.DAS.AdminAan.Application.CalendarEvents.Queries.GetCalendarEvents;

namespace SFA.DAS.AdminAan.Api.Controllers;

[Route("[controller]")]
[ApiController]
public class CalendarEventsController : ControllerBase
{
    private readonly IMediator _mediator;

    public CalendarEventsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [ProducesResponseType(typeof(GetCalendarEventsQueryResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetCalendarEvents([FromQuery] GetCalendarEventsRequestModel requestModel, CancellationToken cancellationToken)
    {
        var response = await _mediator.Send((GetCalendarEventsQuery)requestModel, cancellationToken);
        return Ok(response);
    }

    [HttpPost]
    [ProducesResponseType(typeof(CreateEventCommandResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> PostCalendarEvent([FromBody] CreateEventCommand command, CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(command, cancellationToken);
        return Ok(response);
    }
}
