namespace SFA.DAS.SharedOuterApi.Recruit.Services;

public interface IVacancySlaDeadlineService
{
    Task<DateTime> GetSlaDeadlineAsync(DateTime utcDate, CancellationToken cancellationToken = default);
}

public class VacancySlaDeadlineService(IBankHolidaysService bankHolidaysService): IVacancySlaDeadlineService
{
    private const int SlaHours = 24;
    
    public async Task<DateTime> GetSlaDeadlineAsync(DateTime utcDate, CancellationToken cancellationToken = default)
    {
        var bankHolidayData = await bankHolidaysService.GetBankHolidayDataAsync(cancellationToken);
        var bankHolidays = bankHolidayData.EnglandAndWales.Events
            .Select(e => DateTime.Parse(e.Date))
            .ToList();

        var slaDate = utcDate;

        //Handle vacancies submitted during non-working days
        while (!ValidateSlaDate(slaDate, bankHolidays))
        {
            slaDate = slaDate.Date.AddDays(1);
        }

        //add the SLA duration
        slaDate = slaDate.AddHours(SlaHours);

        //find the next working day
        while(!ValidateSlaDate(slaDate, bankHolidays))
        {
            slaDate = slaDate.AddDays(1);
        }

        return slaDate;
    }
    
    private static bool ValidateSlaDate(DateTime slaDate, IEnumerable<DateTime> bankHolidays)
    {
        if (slaDate.DayOfWeek is DayOfWeek.Saturday or DayOfWeek.Sunday)
        {
            return false;
        }

        return !bankHolidays.Contains(slaDate.Date);
    }
}