﻿using MediatR;
using SFA.DAS.ApprenticeAan.Application.CalendarEvents.Queries.GetCalendarEvents;
using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.ApprenticeAan.Application.CalendarEvents.Queries.GetCalendarEventById;

[ExcludeFromCodeCoverage]
public class GetCalendarEventByIdQuery : IRequest<CalendarEvent>
{
    public Guid CalendarEventId { get; set; }
    public Guid RequestedByMemberId { get; set; }

    public GetCalendarEventByIdQuery(Guid calendarEventId, Guid requestedByMemberId)
    {
        CalendarEventId = calendarEventId;
        RequestedByMemberId = requestedByMemberId;
    }
}
