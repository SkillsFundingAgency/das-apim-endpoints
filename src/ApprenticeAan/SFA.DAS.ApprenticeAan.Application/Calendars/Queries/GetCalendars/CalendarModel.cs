using SFA.DAS.ApprenticeAan.Application.Entities;

namespace SFA.DAS.ApprenticeAan.Application.Calendars.Queries.GetCalendars;
public class CalendarModel
{
    public int Id { get; set; }
    public string CalendarName { get; set; } = string.Empty;
    public DateTime EffectiveFrom { get; set; }
    public DateTime? EffectiveTo { get; set; }
    public int Ordering { get; set; }

    public static implicit operator CalendarModel(Calendar source) =>
        new()
        {
            Id = source.Id,
            CalendarName = source.CalendarName,
            EffectiveFrom = source.EffectiveFrom,
            EffectiveTo = source.EffectiveTo,
            Ordering = source.Ordering
        };
}
