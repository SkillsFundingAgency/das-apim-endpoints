using System.Collections.Generic;
using System.Linq;
using SFA.DAS.FindApprenticeshipTraining.Api.Models;

namespace SFA.DAS.FindApprenticeshipTraining.Api.Extensions
{
    public static class TrainingCourseProviderOrderByExtension
    {
        private static Dictionary<int, decimal> ProviderScores => new Dictionary<int, decimal>();
        
        public static IEnumerable<GetTrainingCourseProviderListItem> OrderByProviderRating(this IEnumerable<GetTrainingCourseProviderListItem> source)
        {
            var getTrainingCourseProviderListItems = source.ToList();

            AddFeedbackRateScore(getTrainingCourseProviderListItems);

            AddAchievementRateScore(getTrainingCourseProviderListItems);
            
            var returnList = ProviderScores.Join(getTrainingCourseProviderListItems,
                    achievementRateScore => achievementRateScore.Key,
                    getTrainingCourseProviderListItem => getTrainingCourseProviderListItem.ProviderId,
                    (achievementRateScore, getTrainingCourseProviderListItem) =>
                        new {achievementRateScore, getTrainingCourseProviderListItem})
                .OrderByDescending(t => t.achievementRateScore.Value)
                .Select(c=>c.getTrainingCourseProviderListItem)
                .OrderByDescending(c=>c.OverallCohort)
                .ThenByDescending(c=>c.Feedback.TotalEmployerResponses)
                .ThenBy(c=>c.Name)
                .ToList();

            return returnList;
        }

        private static void AddAchievementRateScore(IReadOnlyCollection<GetTrainingCourseProviderListItem> getTrainingCourseProviderListItems)
        {
            var totalRates =
                getTrainingCourseProviderListItems.Count(
                    c => c.OverallAchievementRate.HasValue && c.OverallAchievementRate != 0);

            var percentileRate = 100f / (totalRates - 1);

            var score = 100f;
            foreach (var listItem in getTrainingCourseProviderListItems
                .Where(c => c.OverallAchievementRate.HasValue)
                .OrderByDescending(c => c.OverallAchievementRate))
            {
                var scoreValue = GetAchievementRateScore(score);
                if (ProviderScores.ContainsKey(listItem.ProviderId))
                {
                    ProviderScores[listItem.ProviderId] += scoreValue;
                }
                else
                {
                    ProviderScores.Add(listItem.ProviderId, scoreValue);
                }

                score -= percentileRate;
            }

            foreach (var listItem in getTrainingCourseProviderListItems.Where(c => !c.OverallAchievementRate.HasValue))
            {
                if (ProviderScores.ContainsKey(listItem.ProviderId))
                {
                    ProviderScores[listItem.ProviderId] += GetAchievementRateScore(null);
                }
                else
                {
                    ProviderScores.Add(listItem.ProviderId, GetAchievementRateScore(null));
                }
            }
        }

        private static void AddFeedbackRateScore(IEnumerable<GetTrainingCourseProviderListItem> getTrainingCourseProviderListItems)
        {
            foreach (var listItem in getTrainingCourseProviderListItems)
            {
                var feedbackScore = GetFeedbackScore(listItem.Feedback.TotalFeedbackRating);
                ProviderScores.Add(listItem.ProviderId, feedbackScore);
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