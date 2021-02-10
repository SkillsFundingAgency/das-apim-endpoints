using System;
using SFA.DAS.Recruit.InnerApi.Responses;

namespace SFA.DAS.Recruit.Domain
{
    public class TrainingProgramme
    {
        public string Id { get; set; }
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
                Id = source.LarsCode.ToString(),
                ApprenticeshipType = TrainingType.Standard,
                Title = source.Title,
                EffectiveFrom = source.StandardDates.EffectiveFrom,
                EffectiveTo = source.StandardDates.EffectiveTo,
                ApprenticeshipLevel = ApprenticeshipLevelMapper.RemapFromInt(source.Level),
                Duration = source.TypicalDuration,
                IsActive = source.IsActive,
                EducationLevelNumber = source.Level
            };
        }
    }
}
