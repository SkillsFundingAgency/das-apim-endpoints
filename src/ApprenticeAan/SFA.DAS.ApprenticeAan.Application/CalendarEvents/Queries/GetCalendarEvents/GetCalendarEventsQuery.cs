﻿using MediatR;
using SFA.DAS.ApprenticeAan.Application.Common;

namespace SFA.DAS.ApprenticeAan.Application.CalendarEvents.Queries.GetCalendarEvents;
public class GetCalendarEventsQuery : IRequest<GetCalendarEventsQueryResult>
{
    public Guid RequestedByMemberId { get; }
    public string? FromDate { get; }
    public string? ToDate { get; }
    public GetCalendarEventsQuery(Guid requestedByMemberId, DateTime? fromDate, DateTime? toDate, List<EventFormat>? eventFormats)
    {
        RequestedByMemberId = requestedByMemberId;
        FromDate = fromDate?.ToString("yyyy-MM-dd");
        this.ToDate = toDate?.ToString("yyyy-MM-dd");
        EventFormats = eventFormats;
    }
}
