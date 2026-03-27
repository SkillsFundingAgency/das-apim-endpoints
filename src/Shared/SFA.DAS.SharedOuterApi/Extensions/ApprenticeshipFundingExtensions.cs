using System;
using System.Collections.Generic;
using System.Linq;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;

namespace SFA.DAS.SharedOuterApi.Extensions
{
    public static class ApprenticeshipFundingExtensions
    {
        public static int MaxFundingOn(this IEnumerable<ApprenticeshipFunding> funding, DateTime effectiveDate)
        {
            var fundingList = funding?.ToList();

            if (fundingList == null || !fundingList.Any()) return 0;

            var match = fundingList.FirstOrDefault(c =>
                c.EffectiveFrom <= effectiveDate
                && (c.EffectiveTo == null || c.EffectiveTo >= effectiveDate));

            if (match == null)
                match = fundingList.FirstOrDefault(c => c.EffectiveTo == null);

            return match?.MaxEmployerLevyCap
                   ?? fundingList.FirstOrDefault()?.MaxEmployerLevyCap
                   ?? 0;
        }
    }
}
