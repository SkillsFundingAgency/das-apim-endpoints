﻿using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.EmployerAan.Application.Attendances.Commands.PutAttendance;
using SFA.DAS.EmployerAan.Application.CalendarEvents.Queries.GetCalendarEventById;
using SFA.DAS.EmployerAan.Application.CalendarEvents.Queries.GetCalendarEvents;
using SFA.DAS.EmployerAan.Application.Models;
using SFA.DAS.EmployerAan.Infrastructure;
using SFA.DAS.EmployerAan.InnerApi.Attendances;

namespace SFA.DAS.EmployerAan.Api.Controllers;

[ApiController]
[Route("[controller]")]
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

    [HttpGet("{calendarEventId}")]
    [ProducesResponseType(typeof(CalendarEvent), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(CalendarEvent), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetCalendarEventById(
    Guid calendarEventId,
    [FromHeader(Name = Constants.ApiHeaders.RequestedByMemberIdHeader)] Guid requestedByMemberId,
    CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(new GetCalendarEventByIdQuery(calendarEventId, requestedByMemberId), cancellationToken);

        if (response == null) return NotFound();

        return Ok(response);
    }

    [HttpPut("{calendarEventId}/attendance")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> PutAttendance(
    Guid calendarEventId,
    [FromHeader(Name = Constants.ApiHeaders.RequestedByMemberIdHeader)] Guid requestedByMemberId,
    [FromBody] AttendanceStatus request,
    CancellationToken cancellationToken)
    {
        var command = new PutAttendanceCommand(calendarEventId, requestedByMemberId, request);
        await _mediator.Send(command, cancellationToken);
        return NoContent();
    }
}
