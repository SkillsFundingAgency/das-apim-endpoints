namespace SFA.DAS.FindAnApprenticeship.Services
{
    public interface IMetrics
    {
        void IncreaseVacancyViews(string vacancyReference, int viewCount = 1);
        void IncreaseVacancyStarted(string vacancyReference, int viewCount = 1);
        void IncreaseVacancySubmitted(string vacancyReference, int viewCount = 1);
        void IncreaseVacancySearchResultViews(string vacancyReference, int viewCount = 1);
    }
}
