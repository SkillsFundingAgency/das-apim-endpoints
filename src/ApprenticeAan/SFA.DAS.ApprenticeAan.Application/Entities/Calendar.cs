namespace SFA.DAS.ApprenticeAan.Application.Entities;

public class Calendar
{
    public int Id { get; set; }
    public string CalendarName { get; set; } = string.Empty;
    public DateTime EffectiveFromDate { get; set; }
    public DateTime? EffectiveToDate { get; set; }
    public int Ordering { get; set; }
}