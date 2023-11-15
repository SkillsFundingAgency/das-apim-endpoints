using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.AdminAan.Api.Models;
using SFA.DAS.AdminAan.Application.CalendarEvents.Commands.Create;
using SFA.DAS.AdminAan.Application.CalendarEvents.Commands.Delete;
using SFA.DAS.AdminAan.Application.CalendarEvents.Queries.GetCalendarEvent;
using SFA.DAS.AdminAan.Application.CalendarEvents.Queries.GetCalendarEvents;
using SFA.DAS.AdminAan.Infrastructure.Configuration;

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
    [ProducesResponseType(typeof(PostEventCommandResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> PostCalendarEvent([FromHeader(Name = Constants.ApiHeaders.RequestedByMemberIdHeader)] Guid requestedByMemberId, [FromBody] CreateEventRequestModel requestModel, CancellationToken cancellationToken)
    {
        var command = (PostEventCommand)requestModel;
        command.RequestedByMemberId = requestedByMemberId;
        var response = await _mediator.Send(command, cancellationToken);

        return Ok(response);
    }

    [HttpDelete("{calendarEventId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]

    public async Task<IActionResult> DeleteCalendarEvent(Guid calendarEventId, [FromHeader(Name = Constants.ApiHeaders.RequestedByMemberIdHeader)] Guid requestedByMemberId, CancellationToken cancellationToken)
    {
        var command = new DeleteEventCommand
        {
            RequestedByMemberId = requestedByMemberId,
            CalendarEventId = calendarEventId
        };

        await _mediator.Send(command, cancellationToken);

        return NoContent();
    }

    [HttpGet("{calendarEventId}")]
    [ProducesResponseType(typeof(GetCalendarEventQueryResult), StatusCodes.Status200OK)]

    public async Task<IActionResult> GetCalendarEvent([FromHeader(Name = Constants.ApiHeaders.RequestedByMemberIdHeader)] Guid requestedByMemberId, Guid calendarEventId, CancellationToken cancellationToken)
    {
        var query = new GetCalendarEventQuery(requestedByMemberId, calendarEventId);
        var response = await _mediator.Send(query, cancellationToken);

        return Ok(response);
    }
}
