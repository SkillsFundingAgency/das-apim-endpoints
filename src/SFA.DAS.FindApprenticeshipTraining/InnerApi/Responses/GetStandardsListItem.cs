using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using static System.String;

namespace SFA.DAS.FindApprenticeshipTraining.InnerApi.Responses
{
    public class GetStandardsListItem
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int Level { get; set; }
        public string LevelEquivalent { get; set; }
        public decimal Version { get; set; }
        [JsonIgnore]
        public int MaxFunding => GetFundingDetails(nameof(MaxFunding));

        public string OverviewOfRole { get; set; }

        public string Keywords { get; set; }
        [JsonIgnore]
        public int TypicalDuration => GetFundingDetails(nameof(TypicalDuration));

        public string Route { get; set; }

        public string TypicalJobTitles { get; set; }
        public string CoreSkillsCount => GetCoreSkillsCount();

        public string StandardPageUrl { get; set; }

        public string IntegratedDegree { get; set; }
        public string SectorSubjectAreaTier2Description { get; set; }
        public decimal SectorSubjectAreaTier2 { get; set; }
        public bool OtherBodyApprovalRequired { get; set; }
        public string ApprovalBody { get; set; }
        private List<Duty> Duties { get; set; }
        private List<Skill> Skills { get; set; }
        private bool CoreAndOptions { get; set; }

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

        private string GetCoreSkillsCount()
        {
            if (Duties.Any() && Duties != null && Skills.Any() && Skills != null)
            {
                if (CoreAndOptions)
                {
                    var mappedSkillsList = GetMappedSkillsList(this);
                    return GetSkillDetailFromMappedCoreSkill(this, mappedSkillsList);
                }
                return Join("|",
                    Skills.Select(s => s.Detail));
            }

            return null;
        }

        private static IEnumerable<string> GetMappedSkillsList(GetStandardsListItem standard)
        {
            return standard.Duties
                .Where(d => d.IsThisACoreDuty.Equals(1) && d.MappedSkills != null)
                .SelectMany(d => d.MappedSkills)
                .Select(s => s.ToString());
        }

        private static string GetSkillDetailFromMappedCoreSkill(GetStandardsListItem standard, IEnumerable<string> mappedSkillsList)
        {
            return Join("|", standard.Skills
                .Where(s => mappedSkillsList.Contains(s.SkillId))
                .Select(s => s.Detail));
        }
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

    public class Duty
    {
        public long IsThisACoreDuty { get; set; }
        public List<Guid> MappedSkills { get; set; }
    }
    public class Skill
    {
        public string SkillId { get; set; }
        public string Detail { get; set; }
    }
}
