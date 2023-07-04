using MediatR;
using SFA.DAS.ApprenticeAan.Application.Common;

namespace SFA.DAS.ApprenticeAan.Application.CalendarEvents.Queries.GetCalendarEvents;
public class GetCalendarEventsQuery : IRequest<GetCalendarEventsQueryResult>
{
    public Guid RequestedByMemberId { get; set; }

    public string? FromDate { get; set; }

    public string? ToDate { get; set; }

    public List<EventFormat>? EventFormat { get; set; }

    public List<int>? CalendarIds { get; set; }

    public List<int>? RegionIds { get; set; }

    public int? Page { get; set; }

    public int? PageSize { get; set; }
}
