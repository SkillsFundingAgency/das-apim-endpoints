using System;
using System.Linq;
using SFA.DAS.FindApprenticeshipTraining.InnerApi.Responses;

namespace SFA.DAS.FindApprenticeshipTraining.Api.Models
{
    public class GetTrainingCourseProviderListItem
    {
        public string Name { get ; set ; }

        public int ProviderId { get ; set ; }
        public int? OverallCohort { get ; set ; }
        public decimal? OverallAchievementRate { get ; set ; }

        public GetTrainingCourseProviderListItem Map(GetProvidersListItem source, string sectorSubjectArea)
        {
            var achievementRate = source.AchievementRates.FirstOrDefault(c =>
                c.SectorSubjectArea.Equals(sectorSubjectArea, StringComparison.CurrentCultureIgnoreCase));
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