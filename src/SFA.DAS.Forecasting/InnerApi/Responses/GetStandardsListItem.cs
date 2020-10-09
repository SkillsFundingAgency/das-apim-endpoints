using System;
using System.Collections.Generic;
using System.Linq;

namespace SFA.DAS.Forecasting.InnerApi.Responses
{
    public class GetStandardsListItem
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public decimal FundingCap => GetFundingCap();
        public int Level { get; set; }
        public List<FundingPeriod> ApprenticeshipFunding { get; set; }
        public class FundingPeriod
        {
            public DateTime EffectiveFrom { get; set; }
            public DateTime? EffectiveTo { get; set; }
            public int MaxEmployerLevyCap { get; set; }
            public int Duration { get; set; }
        }

        private decimal GetFundingCap()
        {
            var funding = ApprenticeshipFunding
                .FirstOrDefault(c =>
                    c.EffectiveFrom <= DateTime.UtcNow && (c.EffectiveTo == null
                                                           || c.EffectiveTo >= DateTime.UtcNow));
            return funding?.MaxEmployerLevyCap
                   ?? ApprenticeshipFunding.FirstOrDefault()?.MaxEmployerLevyCap
                   ?? 0;
        }
    }
}