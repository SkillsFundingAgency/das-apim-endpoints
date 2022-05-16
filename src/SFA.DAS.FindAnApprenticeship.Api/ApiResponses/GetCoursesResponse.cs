using System;
using System.Collections.Generic;
using System.Linq;
using SFA.DAS.FindAnApprenticeship.Application.Queries.GetCourses;
using SFA.DAS.FindAnApprenticeship.Domain;

namespace SFA.DAS.FindAnApprenticeship.Api.ApiResponses
{
    public class GetCoursesResponse
    {
        public List<GetCourseItem> TrainingProgrammes { get; set; }

        public static implicit operator GetCoursesResponse(GetCoursesQueryResult source)
        {
            return new GetCoursesResponse
            {
                TrainingProgrammes = source.TrainingProgrammes.Select(c=>(GetCourseItem)c).ToList() 
            };
        }
    }

    public class GetCourseItem
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

        public static implicit operator GetCourseItem(TrainingProgramme source)
        {
            return new GetCourseItem
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
    public enum GetTrainingProgrammeTrainingType
    {
        Standard = 0,
        Framework = 1
    }
    public enum GetTrainingProgrammeApprenticeshipLevel
    {
        Unknown = 0,
        Intermediate = 2,
        Advanced = 3,
        Higher = 4,
        Degree = 6
    }
}