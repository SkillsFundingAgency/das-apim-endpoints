using SFA.DAS.AdminAan.Application.CalendarEvents.Queries.GetCalendarEvents;

namespace SFA.DAS.AdminAan.Domain.InnerApi.AanHubApi
{
    public class GetCalendarEventsApiResponse
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
        public int TotalCount { get; set; }
        public IEnumerable<CalendarEventSummary> CalendarEvents { get; set; } = [];
    }
}
