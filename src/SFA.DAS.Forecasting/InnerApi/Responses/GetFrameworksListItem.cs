using System;
using System.Collections.Generic;

namespace SFA.DAS.Forecasting.InnerApi.Responses
{
    public class GetFrameworksListItem
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public decimal CurrentFundingCap { get; set; }
        public int Level { get; set; }
        public int Duration { get; set; }
        public List<FundingPeriod> FundingPeriods { get; set; }
        public bool IsActiveFramework { get; set; }
        public class FundingPeriod
        {
            public DateTime EffectiveFrom { get; set; }
            public DateTime? EffectiveTo { get; set; }
            public int FundingCap { get; set; }
        }
    }
}