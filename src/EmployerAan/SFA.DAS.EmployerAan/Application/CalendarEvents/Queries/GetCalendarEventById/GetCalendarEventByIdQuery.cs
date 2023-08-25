using System.Diagnostics.CodeAnalysis;
using MediatR;
using SFA.DAS.EmployerAan.Application.CalendarEvents.Queries.GetCalendarEvents;

namespace SFA.DAS.EmployerAan.Application.CalendarEvents.Queries.GetCalendarEventById;
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
