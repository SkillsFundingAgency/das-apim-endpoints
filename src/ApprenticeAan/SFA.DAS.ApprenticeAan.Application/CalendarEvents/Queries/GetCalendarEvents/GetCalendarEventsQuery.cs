using MediatR;
using SFA.DAS.ApprenticeAan.Application.Common;

namespace SFA.DAS.ApprenticeAan.Application.CalendarEvents.Queries.GetCalendarEvents;
public class GetCalendarEventsQuery : IRequest<GetCalendarEventsQueryResult>
{
    public Guid RequestedByMemberId { get; }
    public string? FromDate { get; }
    public string? ToDate { get; }
    public List<EventFormat>? EventFormat { get; }
    public List<int>? CalendarIds { get; set; }
    public List<int>? RegionIds { get; set; }

    public GetCalendarEventsQuery(Guid requestedByMemberId, DateTime? fromDate, DateTime? toDate, List<EventFormat>? eventFormat, List<int>? calendarId, List<int>? regionId)
    {
        RequestedByMemberId = requestedByMemberId;
        FromDate = fromDate?.ToString("yyyy-MM-dd");
        ToDate = toDate?.ToString("yyyy-MM-dd");
        EventFormat = eventFormat;
        CalendarIds = calendarId;
        RegionIds = regionId;
    }
}
