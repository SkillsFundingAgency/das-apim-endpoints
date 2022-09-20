namespace SFA.DAS.RoatpCourseManagement.Services.Models
{
    public class NationalAchievementRate : NationalAchievementRateBase
    {
        //public virtual Provider Provider { get ; set ; }
        //public virtual ProviderStandard ProviderStandard { get ; set ; }

        public static implicit operator NationalAchievementRate(NationalAchievementRateImport source)
        {
            return new NationalAchievementRate
            {
                Age = source.Age,
                Ukprn = source.Ukprn,
                ApprenticeshipLevel = source.ApprenticeshipLevel,
                OverallCohort = source.OverallCohort,
                OverallAchievementRate = source.OverallAchievementRate,
                SectorSubjectArea = source.SectorSubjectArea
            };
        }
    }
}