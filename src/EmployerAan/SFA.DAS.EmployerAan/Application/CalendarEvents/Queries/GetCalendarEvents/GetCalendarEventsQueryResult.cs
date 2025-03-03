namespace SFA.DAS.EmployerAan.Application.CalendarEvents.Queries.GetCalendarEvents;
public class GetCalendarEventsQueryResult
{
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int TotalPages { get; set; }
    public int TotalCount { get; set; }
    public IEnumerable<CalendarEventSummary> CalendarEvents { get; set; } = [];
    public bool IsInvalidLocation { get; set; }

    public List<RegionData> Regions { get; set; } = [];
    public List<CalendarType> Calendars { get; set; } = [];

    public class RegionData
    {
        public int Id { get; set; }
        public string Area { get; set; }
        public int Ordering { get; set; }
    }

    public class CalendarType
    {
        public int Id { get; set; }
        public string CalendarName { get; set; } = string.Empty;
        public DateTime EffectiveFrom { get; set; }
        public DateTime? EffectiveTo { get; set; }
        public int Ordering { get; set; }
    }
}
