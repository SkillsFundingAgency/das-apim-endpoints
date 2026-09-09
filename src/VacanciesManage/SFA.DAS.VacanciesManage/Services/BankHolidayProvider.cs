using SFA.DAS.SharedOuterApi.Types.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SFA.DAS.VacanciesManage.Services;

public interface IBankHolidayProvider
{
    Task<List<DateTime>> GetBankHolidaysAsync();
}

public class BankHolidayProvider(IBankHolidaysService service) : IBankHolidayProvider
{
    public async Task<List<DateTime>> GetBankHolidaysAsync()
    {
        var bankHolidayReferenceData = await service.GetBankHolidayData();

        return bankHolidayReferenceData.EnglandAndWales.Events
            .Select(e => DateTime.Parse(e.Date))
            .ToList();
    }
}