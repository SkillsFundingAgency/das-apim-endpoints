using MediatR;

namespace SFA.DAS.AdminAan.Application.CalendarEvents.Queries.GetCalendarEvent;
public class GetCalendarEventQuery : IRequest<GetCalendarEventQueryResult>
{
    public Guid RequestedByMemberId { get; set; }
    public Guid CalendarEventId { get; }

    public GetCalendarEventQuery(Guid requestedByMemberId, Guid calendarEventId)
    {
        RequestedByMemberId = requestedByMemberId;
        CalendarEventId = calendarEventId;
    }
}
