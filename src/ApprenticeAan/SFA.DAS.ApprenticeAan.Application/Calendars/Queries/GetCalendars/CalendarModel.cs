using SFA.DAS.ApprenticeAan.Application.Entities;

namespace SFA.DAS.ApprenticeAan.Application.Calendars.Queries.GetCalendars;
public class CalendarModel
{
    public int Id { get; set; }
    public string CalendarName { get; set; } = string.Empty;
    public DateTime EffectiveFromDate { get; set; }
    public DateTime? EffectiveToDate { get; set; }
    public int Ordering { get; set; }

    public static implicit operator CalendarModel(Calendar source) =>
        new()
        {
            Id = source.Id,
            CalendarName = source.CalendarName,
            EffectiveFromDate = source.EffectiveFromDate,
            EffectiveToDate = source.EffectiveToDate,
            Ordering = source.Ordering
        };
}
