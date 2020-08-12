using System.Linq;

namespace SFA.DAS.FindApprenticeshipTraining.InnerApi.Responses
{
    public class GetProviderCourseItem : ProviderCourseBase
    {
        public int Ukprn { get ; set ; }

        public string Name { get ; set ; }

        public string Email { get ; set ; }

        public string Phone { get ; set ; }

        public string Website { get ; set ; }

        public int? OverallCohort { get; set; }
        public int? NationalOverallCohort { get; set; }
        public decimal? OverallAchievementRate { get ; set ; }
        public decimal? NationalOverallAchievementRate { get ; set ; }

        public GetProviderCourseItem Map(GetProviderStandardItem source, string sectorSubjectArea, int level)
        {
            var achievementRate = GetAchievementRateItem(source.AchievementRates, sectorSubjectArea, level);
            var nationalRate = GetAchievementRateItem(source.OverallAchievementRates, sectorSubjectArea, level);
            
            return new GetProviderCourseItem
            {
                Website = source.ContactUrl,
                Phone = source.Phone,
                Email = source.Email,
                Name = source.Name,
                Ukprn = source.Ukprn,
                OverallCohort = achievementRate?.OverallCohort,
                NationalOverallCohort = nationalRate?.OverallCohort,
                OverallAchievementRate = achievementRate?.OverallAchievementRate,
                NationalOverallAchievementRate = nationalRate?.OverallAchievementRate
            };
        }

        
    }
}