using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace SFA.DAS.FindApprenticeshipTraining.InnerApi.Responses
{
    public class GetStandardsListItem
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int Level { get; set; }
        public decimal Version { get; set; }
        [JsonIgnore] 
        public long MaxFunding => GetMaxFunding();

        public string OverviewOfRole { get; set; }

        public string Keywords { get; set; }

        public int TypicalDuration { get; set; }

        public string Route { get; set; }

        public string TypicalJobTitles { get; set; }

        public string CoreSkillsCount { get; set; }

        public string StandardPageUrl { get; set; }

        public string IntegratedDegree { get; set; }

        private long GetMaxFunding()
        {
            var funding = ApprenticeshipFunding
                .FirstOrDefault(c => 
                    c.EffectiveFrom <= DateTime.UtcNow && (c.EffectiveTo == null 
                                                           || c.EffectiveTo >= DateTime.UtcNow));

            if (funding != null)
            {
                return funding.MaxEmployerLevyCap;
            }
            
            var fundingRecord = ApprenticeshipFunding.FirstOrDefault(c => c.EffectiveTo == null);
        
            return fundingRecord?.MaxEmployerLevyCap 
                                  ?? ApprenticeshipFunding.FirstOrDefault()?.MaxEmployerLevyCap 
                                  ?? 0;
        
        }

        public List<ApprenticeshipFunding> ApprenticeshipFunding { get; set; }
        
        public StandardDate StandardDates { get; set; }
        
    }
    
    public class ApprenticeshipFunding
    {
        public long MaxEmployerLevyCap { get; set; }

        public DateTime? EffectiveTo { get; set; }

        public DateTime EffectiveFrom { get; set; }
    }

    public class StandardDate
    {
        public DateTime? LastDateStarts { get; set; }

        public DateTime? EffectiveTo { get; set; }

        public DateTime EffectiveFrom { get; set; }
    }
}
