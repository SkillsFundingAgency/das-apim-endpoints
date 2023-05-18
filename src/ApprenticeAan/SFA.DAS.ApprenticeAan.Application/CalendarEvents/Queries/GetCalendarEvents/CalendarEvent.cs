namespace SFA.DAS.ApprenticeAan.Application.CalendarEvents.Queries.GetCalendarEvents;
public class CalendarEvent
{
    public Guid Id { get; set; }
    public string? CalendarName { get; set; }
    public string? EventFormat { get; set; }
    public DateTime Start { get; set; }
    public DateTime End { get; set; }

    public string? Description { get; set; }
    public string? Summary { get; set; }
    public string? Location { get; set; }
    public string? Postcode { get; set; }
    public double? Longitude { get; set; }
    public double? Latitude { get; set; }
    public double? Distance { get; set; }
    public bool Attending { get; set; }
}
