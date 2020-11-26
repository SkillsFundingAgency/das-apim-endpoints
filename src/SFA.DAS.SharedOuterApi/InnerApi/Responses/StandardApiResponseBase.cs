using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace SFA.DAS.SharedOuterApi.InnerApi.Responses
{
    public abstract class StandardApiResponseBase
    {
        public List<ApprenticeshipFunding> ApprenticeshipFunding { get; set; }
        
        [JsonIgnore]
        public int MaxFunding => GetFundingDetails(nameof(MaxFunding));
        [JsonIgnore]
        public int TypicalDuration => GetFundingDetails(nameof(TypicalDuration));
        private int GetFundingDetails(string prop)
        {
            var funding = ApprenticeshipFunding
                .FirstOrDefault(c =>
                    c.EffectiveFrom <= DateTime.UtcNow && (c.EffectiveTo == null
                                                           || c.EffectiveTo >= DateTime.UtcNow));

            if (funding == null)
            {
                funding = ApprenticeshipFunding.FirstOrDefault(c => c.EffectiveTo == null);
            }

            if (prop == nameof(MaxFunding))
            {
                return funding?.MaxEmployerLevyCap
                       ?? ApprenticeshipFunding.FirstOrDefault()?.MaxEmployerLevyCap
                       ?? 0;
            }
                
            return funding?.Duration
                   ?? ApprenticeshipFunding.FirstOrDefault()?.Duration
                   ?? 0;
        }
    }
    
    public class ApprenticeshipFunding
    {
        public int MaxEmployerLevyCap { get; set; }

        public DateTime? EffectiveTo { get; set; }

        public DateTime EffectiveFrom { get; set; }
        public int Duration { get; set; }
    }
}