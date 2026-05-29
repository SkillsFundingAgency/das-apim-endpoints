using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SFA.DAS.VacanciesManage.Services;

public interface ISlaService
{
    Task<DateTime> GetSlaDeadlineAsync(DateTime date);
}
public class SlaService(IBankHolidayProvider bankHolidayProvider) : ISlaService
{
    private const int SlaHours = 24;

    public async Task<DateTime> GetSlaDeadlineAsync(DateTime utcDate)
    {
        var bankHolidays = await bankHolidayProvider.GetBankHolidaysAsync();

        var slaDate = utcDate;

        //Handle vacancies submitted during non-working days
        while (!ValidateSlaDate(slaDate, bankHolidays))
        {
            slaDate = slaDate.Date.AddDays(1);
        }

        //add the SLA duration
        slaDate = slaDate.AddHours(SlaHours);

        //find the next working day
        while (!ValidateSlaDate(slaDate, bankHolidays))
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
