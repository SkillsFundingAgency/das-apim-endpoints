using MediatR;
using SFA.DAS.ApprenticeAan.Application.Common;

namespace SFA.DAS.ApprenticeAan.Application.CalendarEvents.Queries.GetCalendarEvents;
public class GetCalendarEventsQuery : IRequest<GetCalendarEventsQueryResult>
{
    public Guid RequestedByMemberId { get; }
    public string? FromDate { get; }
    public string? ToDate { get; }
    public List<EventFormat>? EventFormat { get; }
    public GetCalendarEventsQuery(Guid requestedByMemberId, DateTime? fromDate, DateTime? toDate, List<EventFormat>? eventFormat)
    {
        RequestedByMemberId = requestedByMemberId;
        FromDate = fromDate?.ToString("yyyy-MM-dd");
        ToDate = toDate?.ToString("yyyy-MM-dd");
        EventFormat = eventFormat;
    }
}
