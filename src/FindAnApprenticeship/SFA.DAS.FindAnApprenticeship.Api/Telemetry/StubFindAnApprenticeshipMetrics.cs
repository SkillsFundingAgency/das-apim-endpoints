using SFA.DAS.FindAnApprenticeship.Services;

namespace SFA.DAS.FindAnApprenticeship.Api.Telemetry
{
    public class StubFindAnApprenticeshipMetrics : IMetrics
    {
        public void IncreaseVacancyViews(string vacancyReference, int viewCount = 1)
        {
        }

        public void IncreaseVacancyStarted(string vacancyReference, int viewCount = 1)
        {
        }

        public void IncreaseVacancySubmitted(string vacancyReference, int viewCount = 1)
        {
        }

        public void IncreaseVacancySearchResultViews(string vacancyReference, int viewCount = 1)
        {
        }
    }
}
