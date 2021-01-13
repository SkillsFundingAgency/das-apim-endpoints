using System;
using System.Linq;
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
                //todo ApprenticeshipLevel = MapApprenticeshipLevel(source.Level),
                Duration = source.Duration,
                IsActive = false,
                //todo EducationLevelNumber = source.Level
            };
        }

        public static implicit operator TrainingProgramme(GetStandardsListItem source)
        {
            return new TrainingProgramme
            {
                Id = source.Id.ToString(),
                ApprenticeshipType = TrainingType.Standard,
                Title = source.Title,
                EffectiveFrom = source.StandardDates.EffectiveFrom,
                EffectiveTo = source.StandardDates.EffectiveTo,
                //todo ApprenticeshipLevel = MapApprenticeshipLevel(source.Level),
                Duration = source.TypicalDuration,
                //todo IsActive = false,
                //todo EducationLevelNumber = source.Level
            };
        }

        private static int MapApprenticeshipLevel(int value)
        {
            switch (value)
            {
                case 5: // Foundation Degree
                    value = (int)ApprenticeshipLevel.Higher;
                    break;
                case 7: // Masters
                    value = (int)ApprenticeshipLevel.Degree;
                    break;
            }
            if (Enum.IsDefined(typeof(ApprenticeshipLevel), value))
            {
                return value;
            }

            return 0;
        }

        private static bool IsStandardActive(GetStandardsListItem standard)
        {
            return standard.StandardDates.EffectiveFrom.Date <= DateTime.UtcNow.Date
                   && (!standard.StandardDates.EffectiveTo.HasValue ||
                       standard.StandardDates.EffectiveTo.Value.Date >= DateTime.UtcNow.Date);
        }
    }
}
