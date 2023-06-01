using MediatR;

namespace SFA.DAS.ApprenticeAan.Application.CalendarEvents.Queries.GetCalendarEvents;
public class GetCalendarEventsQuery : IRequest<GetCalendarEventsQueryResult>
{
    public Guid RequestedByMemberId { get; }
    public string? StartDate { get; }
    public string? EndDate { get; }
    public GetCalendarEventsQuery(Guid requestedByMemberId, DateTime? startDate, DateTime? endDate)
    {
        RequestedByMemberId = requestedByMemberId;
        StartDate = startDate?.ToString("yyyy-MM-dd");
        EndDate = endDate?.ToString("yyyy-MM-dd");
    }
}
