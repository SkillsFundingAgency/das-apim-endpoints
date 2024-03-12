using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindApprenticeshipTraining.InnerApi.Requests
{
    public class GetOverallAchievementRateRequest : IGetApiRequest
    {
        private readonly int _sectorSubjectAreaTier1;

        public GetOverallAchievementRateRequest(int sectorSubjectAreaTier1)
        {
            _sectorSubjectAreaTier1 = sectorSubjectAreaTier1;
        }

        public string GetUrl => $"api/AchievementRates/Overall?sectorSubjectAreaTier1Code={_sectorSubjectAreaTier1}";
    }
}