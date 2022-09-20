using SFA.DAS.RoatpCourseManagement.Services.Extensions;
using SFA.DAS.RoatpCourseManagement.Services.Models.ImportTypes;

namespace SFA.DAS.RoatpCourseManagement.Services.Models
{
    public class NationalAchievementRateOverallImport : NationalAchievementRateOverallBase
    {
        public static implicit operator NationalAchievementRateOverallImport(NationalAchievementRateOverallCsv source)
        {
            var overallCohortResult = int.TryParse(source.OverallCohort, out var overallCohort);
            var overallAchievementRateResult = decimal.TryParse(source.OverallAchievementRate, out var overallAchievementRate);
            
            return new NationalAchievementRateOverallImport
            {
                SectorSubjectArea = source.SectorSubjectArea,
                OverallCohort = !overallCohortResult ? (int?)null : overallCohort,
                OverallAchievementRate = !overallAchievementRateResult ? (decimal?)null : overallAchievementRate,
                Age = source.Age.ToAge(),
                ApprenticeshipLevel = source.ApprenticeshipLevel.ToApprenticeshipLevel(),    
            };
        }
    }
}