using System;
using SFA.DAS.Recruit.InnerApi.Responses;

namespace SFA.DAS.Recruit.Domain
{
    public class TrainingProgramme
    {
        public string Id { get; set; }
        public string LarsCode { get; set; }
        public string StandardUId { get; set; }
        public TrainingType ApprenticeshipType { get; set; }
        public string Title { get; set; }
        public DateTime? EffectiveFrom { get; set; }
        public DateTime? EffectiveTo { get; set; }
        public ApprenticeshipLevel ApprenticeshipLevel { get; set; }
        public int Duration { get; set; }
        public bool IsActive { get; set; }
        public int? EducationLevelNumber { get; set; }

        public static implicit operator TrainingProgramme(GetFrameworksListItem source)
        {
            return new TrainingProgramme
            {
                Id = source.Id,
                ApprenticeshipType = TrainingType.Framework,
                Title = source.Title,
                EffectiveFrom = source.EffectiveFrom,
                EffectiveTo = source.EffectiveTo,
                ApprenticeshipLevel = ApprenticeshipLevelMapper.RemapFromInt(source.Level),
                Duration = source.Duration,
                IsActive = false,
                EducationLevelNumber = source.Level
            };
        }

        public static implicit operator TrainingProgramme(GetStandardsListItem source)
        {
            return new TrainingProgramme
            {
                // ID should be StandardUID but the consumers need to be changed to use lars code
                // before the switch.
                // LarsCode also string so as not to return a larscode:0 field in the framework items as the endpoint
                // for recruit returns both.
                Id = source.LarsCode.ToString(),
                StandardUId = source.StandardUId,
                LarsCode = source.LarsCode.ToString(),
                ApprenticeshipType = TrainingType.Standard,
                Title = source.Title,
                EffectiveFrom = source.StandardDates.EffectiveFrom,
                EffectiveTo = source.StandardDates.EffectiveTo,
                ApprenticeshipLevel = ApprenticeshipLevelMapper.RemapFromInt(source.Level),
                Duration = source.TypicalDuration,
                IsActive = IsStandardActiveMapper.IsStandardActive(source),
                EducationLevelNumber = source.Level
            };
        }
    }
}
