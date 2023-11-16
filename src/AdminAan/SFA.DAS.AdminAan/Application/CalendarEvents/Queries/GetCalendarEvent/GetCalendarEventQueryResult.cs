namespace SFA.DAS.AdminAan.Application.CalendarEvents.Queries.GetCalendarEvent;
public class GetCalendarEventQueryResult
{
    public Guid CalendarEventId { get; set; }
    public string? CalendarName { get; set; } = null!;
    public string EventFormat { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string? Summary { get; set; }
    public string? Location { get; set; }
    public string? Postcode { get; set; }
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }
    public string? EventLink { get; set; }
    public string ContactName { get; set; } = string.Empty;
    public string ContactEmail { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public DateTime? LastUpdatedDate { get; set; }
    public IEnumerable<AttendeeModel> Attendees { get; set; } = null!;
    public IEnumerable<EventGuestModel> EventGuests { get; set; } = null!;

    public int PlannedAttendees { get; set; }
    public DateTime? CreatedDate { get; set; }

    public long? Urn { get; set; }
    public string? SchoolName { get; set; }

    public int? RegionId { get; set; }
    public string? RegionName { get; set; }
}