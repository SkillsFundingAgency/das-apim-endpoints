namespace SFA.DAS.ApprenticeAan.Application.CalendarEvents.Queries.GetCalendarEvents;
public class GetCalendarEventsQueryResult
{
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int TotalPages { get; set; }
    public int TotalCount { get; set; }
    public IEnumerable<CalendarEvent> CalendarEvents { get; set; } = Enumerable.Empty<CalendarEvent>();
}