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
        public int SectorCode { get ; set ; }
        public int FrameworkCode { get ; set ; }
        public int Ssa1 { get ; set ; }

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
                EducationLevelNumber = source.Level,
                SectorCode = 0,
                FrameworkCode = source.FrameworkCode,
                Ssa1 = source.Ssa1,
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
                EducationLevelNumber = source.Level,
                SectorCode = source.SectorCode,
                FrameworkCode = 0,
                Ssa1 = 0
            };
        }
    }
}
