using System;
using SFA.DAS.Recruit.InnerApi.Responses;

namespace SFA.DAS.Recruit.Domain
{
    public static class IsStandardActiveMapper
    {
        public static bool IsStandardActive(GetStandardsListItem standard)
        {
            return standard.StandardDates.EffectiveFrom.Date <= DateTime.UtcNow.Date
                   && (!standard.StandardDates.EffectiveTo.HasValue ||
                       standard.StandardDates.EffectiveTo.Value.Date >= DateTime.UtcNow.Date);
        }
    }
}