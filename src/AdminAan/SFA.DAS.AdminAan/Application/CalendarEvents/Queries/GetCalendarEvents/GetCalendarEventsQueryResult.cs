using SFA.DAS.AdminAan.Application.Entities;
using SFA.DAS.SharedOuterApi.Models;

namespace SFA.DAS.AdminAan.Application.CalendarEvents.Queries.GetCalendarEvents;
public class GetCalendarEventsQueryResult
{
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int TotalPages { get; set; }
    public int TotalCount { get; set; }
    public IEnumerable<CalendarEventSummary> CalendarEvents { get; set; } = [];
    public bool IsInvalidLocation { get; set; }

    public List<Region> Regions { get; set; } = [];
    public List<Calendar> Calendars { get; set; } = [];

    public class Region
    {
        public int Id { get; set; }
        public string Area { get; set; }
        public int Ordering { get; set; }
    }

}
