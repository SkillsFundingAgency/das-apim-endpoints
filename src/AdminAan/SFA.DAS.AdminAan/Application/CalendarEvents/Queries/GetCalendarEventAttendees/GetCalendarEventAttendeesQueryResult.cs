namespace SFA.DAS.AdminAan.Application.CalendarEvents.Queries.GetCalendarEventAttendees;

public class GetCalendarEventAttendeesQueryResult
{
    public List<Attendee> Attendees { get; set; }

    public class Attendee
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public DateTime SignUpDate { get; set; }
    }

}