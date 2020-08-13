using System;
using System.Linq;
using SFA.DAS.FindApprenticeshipTraining.InnerApi.Responses;

namespace SFA.DAS.FindApprenticeshipTraining.Api.Models
{
    public class GetTrainingCourseProviderListItem : ProviderCourseBase
    {
        public string Name { get ; set ; }

        public int ProviderId { get ; set ; }
        public int? OverallCohort { get ; set ; }
        public decimal? OverallAchievementRate { get ; set ; }

        public GetTrainingCourseProviderListItem Map(GetProvidersListItem source, string sectorSubjectArea, int level)
        {
            var achievementRate = GetAchievementRateItem(source.AchievementRates, sectorSubjectArea, level);
            
            return new GetTrainingCourseProviderListItem
            {
                Name = source.Name,
                ProviderId = source.Ukprn,
                OverallCohort = achievementRate?.OverallCohort,
                OverallAchievementRate = achievementRate?.OverallAchievementRate
            };
        }
    }
}