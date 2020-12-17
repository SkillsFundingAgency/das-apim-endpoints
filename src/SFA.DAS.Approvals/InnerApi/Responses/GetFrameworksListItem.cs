using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace SFA.DAS.Approvals.InnerApi.Responses
{
    public class GetFrameworksListItem
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string FrameworkName { get; set; }
        public string PathwayName { get; set; }
        public decimal CurrentFundingCap { get; set; }
        public int FrameworkCode { get; set; }
        public int ProgType { get; set; }
        public int PathwayCode { get; set; }
        public int Level { get; set; }
        public int Duration { get; set; }
        public List<FundingPeriod> FundingPeriods { get; set; }
        public bool IsActiveFramework { get; set; }
        public DateTime EffectiveFrom { get; set; }
        public DateTime? EffectiveTo { get; set; }
        public class FundingPeriod
        {
            public DateTime EffectiveFrom { get; set; }
            public DateTime? EffectiveTo { get; set; }
            [JsonProperty("FundingCap")]
            public int MaxEmployerLevyCap { get; set; }
        }
    }
}