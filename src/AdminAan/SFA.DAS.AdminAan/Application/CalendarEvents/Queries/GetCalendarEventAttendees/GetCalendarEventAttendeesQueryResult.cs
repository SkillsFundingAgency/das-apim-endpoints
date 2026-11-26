namespace SFA.DAS.AdminAan.Application.CalendarEvents.Queries.GetCalendarEventAttendees;

public class GetCalendarEventAttendeesQueryResult
{
    public List<Attendee> Attendees { get; set; } = new List<Attendee>();

    public class Attendee
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public DateTime SignUpDate { get; set; }
    }

}