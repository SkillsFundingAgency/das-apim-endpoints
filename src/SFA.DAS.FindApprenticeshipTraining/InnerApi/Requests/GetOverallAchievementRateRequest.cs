using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindApprenticeshipTraining.InnerApi.Requests
{
    public class GetOverallAchievementRateRequest : IGetApiRequest
    {
        private readonly string _sectorSubjectAreaTier2Description;

        public GetOverallAchievementRateRequest(string sectorSubjectAreaTier2Description)
        {
            _sectorSubjectAreaTier2Description = sectorSubjectAreaTier2Description;
        }

        public string GetUrl => $"api/AchievementRates/Overall?sector={_sectorSubjectAreaTier2Description}";
    }
}