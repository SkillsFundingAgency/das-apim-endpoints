namespace SFA.DAS.AdminAan.Domain.InnerApi.AanHubApi.Responses
{
    public class GetCalendarEventByIdApiResponse
    {
        public Guid CalendarEventId { get; set; }
        public string? CalendarName { get; set; } = null!;
        public int CalendarId { get; set; }
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
        public List<Attendee> Attendees { get; set; } = null!;
        public List<CancelledAttendee> CancelledAttendees { get; set; } = null!;
        public List<EventGuest> EventGuests { get; set; } = null!;
        public int PlannedAttendees { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? RegionId { get; set; }
        public long? Urn { get; set; }
    }

    public record Attendee(Guid MemberId, string UserType, string MemberName, string Surname, string Email, DateTime? AddedDate, DateTime? CancelledDate);

    public record CancelledAttendee(Guid MemberId, string UserType, string MemberName, string Email, DateTime? AddedDate, DateTime? CancelledDate);

    public record EventGuest(string GuestName, string GuestJobTitle);

}
