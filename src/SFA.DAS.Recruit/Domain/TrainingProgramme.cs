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
                Id = source.Id
            };
        }

        public static implicit operator TrainingProgramme(GetStandardsListItem source)
        {
            return new TrainingProgramme
            {
                Id = source.Id.ToString()
            };
        }
    }
}
