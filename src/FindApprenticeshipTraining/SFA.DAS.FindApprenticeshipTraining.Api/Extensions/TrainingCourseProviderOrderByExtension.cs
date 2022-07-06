using System.Collections.Generic;
using System.Linq;
using SFA.DAS.FindApprenticeshipTraining.Api.Models;

namespace SFA.DAS.FindApprenticeshipTraining.Api.Extensions
{
    public static class TrainingCourseProviderOrderByExtension
    {
        private static Dictionary<int, decimal> _providerScores;
        private static DeliveryModeType _filteredDeliveryMode;

        public static IEnumerable<GetTrainingCourseProviderListItem> OrderByProviderScore(
            this IEnumerable<GetTrainingCourseProviderListItem> source, List<DeliveryModeType> requestDeliveryModes = null)
        {
             
            _providerScores = new Dictionary<int, decimal>();
            
            var getTrainingCourseProviderListItems = source.ToList();

            AddFeedbackRateScore(getTrainingCourseProviderListItems);

            AddAchievementRateScore(getTrainingCourseProviderListItems);

            var scoredAndSortedProviders = 
                _providerScores.Join(getTrainingCourseProviderListItems,
                    achievementRateScore => achievementRateScore.Key,
                    getTrainingCourseProviderListItem => getTrainingCourseProviderListItem.ProviderId,
                    (achievementRateScore, getTrainingCourseProviderListItem) =>
                        new {achievementRateScore, getTrainingCourseProviderListItem})
                .OrderByDescending(t => t.achievementRateScore.Value)
                .Select(c=>
                {
                    var getTrainingCourseProviderListItem = c.getTrainingCourseProviderListItem;
                    getTrainingCourseProviderListItem.Score = c.achievementRateScore.Value;
                    return getTrainingCourseProviderListItem;
                })
                .OrderByDescending(c=>c.Score)
                .ThenByDescending(c=>c.OverallAchievementRate)
                .ThenByDescending(c=>c.OverallCohort)
                .ThenByDescending(c=>c.Feedback.TotalEmployerResponses)
                .ThenBy(c=>c.Name)
                .ToList();

            _filteredDeliveryMode = DeliveryModeType.NotFound;
            if (requestDeliveryModes !=null  && requestDeliveryModes.Count(c => c!=DeliveryModeType.National) == 1) //TODO need to exclude national
            {
                _filteredDeliveryMode = requestDeliveryModes.First(c => c!=DeliveryModeType.National);
            }

            if (!getTrainingCourseProviderListItems.All(c => c.HasLocation) 
                || getTrainingCourseProviderListItems.All(c=>c.DeliveryModes.All(x=>x.DeliveryModeType == DeliveryModeType.Workplace))
                || _filteredDeliveryMode == DeliveryModeType.Workplace)
            {
                return scoredAndSortedProviders;
            }

            return scoredAndSortedProviders.OrderByScoreAndDistance(_filteredDeliveryMode);
        }
        private static void AddAchievementRateScore(IReadOnlyCollection<GetTrainingCourseProviderListItem> getTrainingCourseProviderListItems)
        {
            
            var providerAchievementRates
                = getTrainingCourseProviderListItems.Where(
                c => c.OverallAchievementRate.HasValue && c.OverallAchievementRate != 0)
                    .ToList();

            var distinctAchievementRates = providerAchievementRates
                .Select(c=>c.OverallAchievementRate)
                .Distinct()
                .ToList();
            
            var distinctRateCount = (double) distinctAchievementRates
                .Count - 1;
            
            var proportionOfValues = providerAchievementRates.Select(value => new 
            {
                Value = value,
                Proportion = providerAchievementRates
                    .Count(x => value.OverallAchievementRate > x.OverallAchievementRate) / distinctRateCount
            });

            var percentileValues = distinctAchievementRates
                .Select(u => new {
                Value = u ?? 0,
                Proportion = proportionOfValues
                    .Where(v => v.Value.OverallAchievementRate == u)
                    .Select(x => x.Proportion)
                    .Sum() * 100
            }).ToList();

            if (getTrainingCourseProviderListItems.Count(c => c.OverallAchievementRate.HasValue) == 1)
            {
                _providerScores[
                    getTrainingCourseProviderListItems.First(c => c.OverallAchievementRate.HasValue).ProviderId] += 4;
            }
            else
            {
                foreach (var listItem in getTrainingCourseProviderListItems
                    .Where(c => c.OverallAchievementRate.HasValue)
                    .OrderByDescending(c => c.OverallAchievementRate))
                {
                    var score = percentileValues.FirstOrDefault(x => x.Value == listItem.OverallAchievementRate); 
                    var scoreValue = GetAchievementRateScore((float)(score?.Proportion??0));
                
                    _providerScores[listItem.ProviderId] += scoreValue;
                
                }    
            }
            

            foreach (var listItem in getTrainingCourseProviderListItems.Where(c => !c.OverallAchievementRate.HasValue))
            {
                _providerScores[listItem.ProviderId] += GetAchievementRateScore(null);
            }
        }
        private static void AddFeedbackRateScore(IEnumerable<GetTrainingCourseProviderListItem> getTrainingCourseProviderListItems)
        {
            foreach (var listItem in getTrainingCourseProviderListItems)
            {
                var feedbackScore = GetFeedbackScore(listItem.Feedback.TotalFeedbackRating);
                _providerScores.Add(listItem.ProviderId, feedbackScore);
            }
        }
        private static decimal GetFeedbackScore(int score)
        {
            return score switch
            {
                4 => 8,
                3 => 7,
                2 => -1.5m,
                1 => -3,
                _ => 6
            };
        }
        private static int GetAchievementRateScore(float? rate)
        {
            if (!rate.HasValue)
            {
                return 1;
            }

            if (rate > 70)
            {
                return 4;
            }
            if (rate > 60)
            {
                return 3;
            }
            if (rate > 50)
            {
                return 2;
            }
            if (rate > 40)
            {
                return 1;
            }
            if (rate > 30)
            {
                return 0;
            }

            return -1;

        }
        
    }
}