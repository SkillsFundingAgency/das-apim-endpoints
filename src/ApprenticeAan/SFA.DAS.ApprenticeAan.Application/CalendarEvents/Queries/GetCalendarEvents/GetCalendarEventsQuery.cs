using MediatR;

namespace SFA.DAS.ApprenticeAan.Application.CalendarEvents.Queries.GetCalendarEvents;
public class GetCalendarEventsQuery : IRequest<GetCalendarEventsQueryResult>
{
    public Guid RequestedByMemberId { get; }
    public GetCalendarEventsQuery(Guid requestedByMemberId) => RequestedByMemberId = requestedByMemberId;
}
