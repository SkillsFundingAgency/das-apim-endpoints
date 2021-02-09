using System;
using SFA.DAS.Recruit.Domain;

namespace SFA.DAS.Recruit.Api.Models
{
    public class GetTrainingProgrammeResponse
    {
        public string Id { get; set; }
        public string LarsCode { get; set; }
        public string StandardUId { get; set; }
        public GetTrainingProgrammeTrainingType ApprenticeshipType { get; set; }
        public string Title { get; set; }
        public DateTime? EffectiveFrom { get; set; }
        public DateTime? EffectiveTo { get; set; }
        public GetTrainingProgrammeApprenticeshipLevel ApprenticeshipLevel { get; set; }
        public int Duration { get; set; }
        public bool IsActive { get; set; }
        public int? EducationLevelNumber { get; set; }
        
        

        public static implicit operator GetTrainingProgrammeResponse(TrainingProgramme source)
        {
            return new GetTrainingProgrammeResponse
            {
                Id = source.Id,
                LarsCode = source.LarsCode,
                StandardUId = source.StandardUId,
                ApprenticeshipType = (GetTrainingProgrammeTrainingType)source.ApprenticeshipType,
                Title = source.Title,
                EffectiveFrom = source.EffectiveFrom,
                EffectiveTo = source.EffectiveTo,
                ApprenticeshipLevel = (GetTrainingProgrammeApprenticeshipLevel)source.ApprenticeshipLevel,
                Duration = source.Duration,
                IsActive = source.IsActive,
                EducationLevelNumber = source.EducationLevelNumber,
                
            };
        }
    }
}
