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
        public int MaxFunding => GetFundingDetails(nameof(MaxFunding));

        public string OverviewOfRole { get; set; }

        public string Keywords { get; set; }
        [JsonIgnore]
        public int TypicalDuration => GetFundingDetails(nameof(TypicalDuration));

        public string Route { get; set; }

        public string TypicalJobTitles { get; set; }

        public string CoreSkillsCount { get; set; }

        public string StandardPageUrl { get; set; }

        public string IntegratedDegree { get; set; }
        public string SectorSubjectAreaTier2Description { get; set; }
        public decimal SectorSubjectAreaTier2 { get; set; }
        public bool OtherBodyApprovalRequired { get; set; }

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
        
        public List<ApprenticeshipFunding> ApprenticeshipFunding { get; set; }

        public StandardDate StandardDates { get; set; }

    }

    public class ApprenticeshipFunding
    {
        public int MaxEmployerLevyCap { get; set; }

        public DateTime? EffectiveTo { get; set; }

        public DateTime EffectiveFrom { get; set; }
        public int Duration { get; set; }
    }

    public class StandardDate
    {
        public DateTime? LastDateStarts { get; set; }

        public DateTime? EffectiveTo { get; set; }

        public DateTime EffectiveFrom { get; set; }
    }
}
