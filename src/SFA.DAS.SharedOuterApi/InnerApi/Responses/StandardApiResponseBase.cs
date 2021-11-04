using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace SFA.DAS.SharedOuterApi.InnerApi.Responses
{
    public abstract class StandardApiResponseBase
    {
        public StandardDate StandardDates { get; set; }
        public List<ApprenticeshipFunding> ApprenticeshipFunding { get; set; }
        
        [JsonIgnore]
        public int MaxFunding => GetFundingDetails(nameof(MaxFunding));
        [JsonIgnore]
        public int TypicalDuration => GetFundingDetails(nameof(TypicalDuration));
        [JsonIgnore] 
        public bool IsActive => IsStandardActive();

        public int MaxFundingOn(DateTime effectiveDate)
        {
            return GetFundingDetails(nameof(MaxFunding), effectiveDate);
        }

        protected virtual int GetFundingDetails(string prop, DateTime? effectiveDate = null)
        {
            if (ApprenticeshipFunding == null || !ApprenticeshipFunding.Any()) return 0;

            var effDate = effectiveDate ?? DateTime.UtcNow;

            var funding = ApprenticeshipFunding
                .FirstOrDefault(c =>
                    c.EffectiveFrom <= effDate
                    && (c.EffectiveTo == null || c.EffectiveTo >= effDate));

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

        private bool IsStandardActive()
        {
            if (StandardDates == null) return false;

            return StandardDates.EffectiveFrom.Date <= DateTime.UtcNow.Date
                   && (!StandardDates.EffectiveTo.HasValue ||
                       StandardDates.EffectiveTo.Value.Date >= DateTime.UtcNow.Date);
        }
    }
    
    public class StandardDate
    {
        public DateTime? LastDateStarts { get; set; }

        public DateTime? EffectiveTo { get; set; }

        public DateTime EffectiveFrom { get; set; }
    }
    
    public class ApprenticeshipFunding
    {
        public int MaxEmployerLevyCap { get; set; }

        public DateTime? EffectiveTo { get; set; }

        public DateTime EffectiveFrom { get; set; }
        public int Duration { get; set; }
    }
}