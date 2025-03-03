using MediatR;

namespace SFA.DAS.AdminAan.Application.CalendarEvents.Queries.GetCalendarEventAttendees
{
    public class GetCalendarEventAttendeesQuery(Guid requestedByMemberId, Guid calendarEventId)
        : IRequest<GetCalendarEventAttendeesQueryResult>
    {
        public Guid RequestedByMemberId { get; } = requestedByMemberId;
        public Guid CalendarEventId { get; } = calendarEventId;
    }
}
