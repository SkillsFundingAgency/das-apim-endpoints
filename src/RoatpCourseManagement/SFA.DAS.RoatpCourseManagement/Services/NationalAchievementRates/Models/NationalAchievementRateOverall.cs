using SFA.DAS.RoatpCourseManagement.Services.Extensions;
using SFA.DAS.RoatpCourseManagement.Services.Models.ImportTypes;

namespace SFA.DAS.RoatpCourseManagement.Services.Models
{
    public class NationalAchievementRateOverall 
    {
        public Age Age { get; set; }
        public string SectorSubjectArea { get; set; }
        public ApprenticeshipLevel ApprenticeshipLevel { get; set; }
        public int? OverallCohort { get; set; }
        public decimal? OverallAchievementRate { get; set; }

        public static implicit operator NationalAchievementRateOverall(NationalAchievementRateOverallCsv source)
        {
            var overallCohortResult = int.TryParse(source.OverallCohort, out var overallCohort);
            var overallAchievementRateResult = decimal.TryParse(source.OverallAchievementRate, out var overallAchievementRate);

            return new NationalAchievementRateOverall
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