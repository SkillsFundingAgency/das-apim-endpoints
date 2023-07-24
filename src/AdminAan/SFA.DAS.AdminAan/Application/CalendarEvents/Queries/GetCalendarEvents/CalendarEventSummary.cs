namespace SFA.DAS.AdminAan.Application.CalendarEvents.Queries.GetCalendarEvents;
public class CalendarEventSummary
{
    public Guid CalendarEventId { get; set; }
    public string CalendarName { get; set; } = null!;

    public DateTime Start { get; set; }
    public string Title { get; set; } = null!;

    public bool IsActive { get; set; }
    public int NumberOfAttendees { get; set; }


}