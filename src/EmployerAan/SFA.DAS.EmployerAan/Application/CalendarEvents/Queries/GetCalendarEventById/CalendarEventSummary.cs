using SFA.DAS.EmployerAan.Models;

namespace SFA.DAS.EmployerAan.Application.CalendarEvents.Queries.GetCalendarEvents;
public class CalendarEvent
{
    public Guid CalendarEventId { get; set; }
    public string CalendarName { get; set; } = null!;
    public string EventFormat { get; set; } = null!;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string? Summary { get; set; }
    public int? RegionId { get; set; }
    public string? Location { get; set; }
    public string? Postcode { get; set; }
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }
    public string? EventLink { get; set; }
    public string ContactName { get; set; } = string.Empty;
    public string ContactEmail { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public string? CancelReason { get; set; } = null!;
    public IEnumerable<Attendee> Attendees { get; set; } = null!;
    public IEnumerable<EventGuest> EventGuests { get; set; } = null!;
}
