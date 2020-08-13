using System.Collections.Generic;

namespace SFA.DAS.FindApprenticeshipTraining.InnerApi.Responses
{
    public class GetOverallAchievementRateResponse
    {
        public IEnumerable<GetAchievementRateItem> OverallAchievementRates { get; set; }
    }
}