using System.Collections.Generic;

namespace SFA.DAS.EmployerFinance.InnerApi.Responses.Finance;

public sealed record GetExpiredLevyResponse
{
    /// <summary>ExpiredFund (transaction type 5) - the 24-month expiry.</summary>
    public List<CalendarPeriodAmount> LongTerm { get; init; } = [];

    /// <summary>ShortExpiredFund (transaction type 6) - the 12-month expiry.</summary>
    public List<CalendarPeriodAmount> ShortTerm { get; init; } = [];
}
