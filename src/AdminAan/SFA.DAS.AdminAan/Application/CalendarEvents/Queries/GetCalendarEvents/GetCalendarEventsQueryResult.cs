﻿namespace SFA.DAS.AdminAan.Application.CalendarEvents.Queries.GetCalendarEvents;
public class GetCalendarEventsQueryResult
{
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int TotalPages { get; set; }
    public int TotalCount { get; set; }
    public IEnumerable<CalendarEventSummary> CalendarEvents { get; set; } = Enumerable.Empty<CalendarEventSummary>();
}
