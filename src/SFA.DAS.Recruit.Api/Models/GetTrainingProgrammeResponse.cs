using System;
using SFA.DAS.Recruit.Domain;

namespace SFA.DAS.Recruit.Api.Models
{
    public class GetTrainingProgrammeResponse
    {
        public string Id { get; set; }
        public GetTrainingProgrammeTrainingType ApprenticeshipType { get; set; }
        public string Title { get; set; }
        public DateTime? EffectiveFrom { get; set; }
        public DateTime? EffectiveTo { get; set; }
        public GetTrainingProgrammeApprenticeshipLevel ApprenticeshipLevel { get; set; }
        public int Duration { get; set; }
        public bool IsActive { get; set; }
        public int? EducationLevelNumber { get; set; }
        public int Ssa1 { get ; set ; }
        public int FrameworkCode { get ; set ; }
        public int SectorCode { get ; set ; }
        public string Route { get; set; }

        public static implicit operator GetTrainingProgrammeResponse(TrainingProgramme source)
        {
            return new GetTrainingProgrammeResponse
            {
                Id = source.Id,
                ApprenticeshipType = (GetTrainingProgrammeTrainingType)source.ApprenticeshipType,
                Title = source.Title,
                EffectiveFrom = source.EffectiveFrom,
                EffectiveTo = source.EffectiveTo,
                ApprenticeshipLevel = (GetTrainingProgrammeApprenticeshipLevel)source.ApprenticeshipLevel,
                Duration = source.Duration,
                IsActive = source.IsActive,
                EducationLevelNumber = source.EducationLevelNumber,
                SectorCode = source.SectorCode,
                FrameworkCode = source.FrameworkCode,
                Ssa1 = source.Ssa1,
                Route = source.Route
            };
        }
    }
}
