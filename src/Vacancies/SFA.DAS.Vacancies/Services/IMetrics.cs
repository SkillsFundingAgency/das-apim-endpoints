namespace SFA.DAS.Vacancies.Services
{
    public interface IMetrics
    {
        void IncreaseVacancyViews(string vacancyReference, int viewCount = 1);
        void IncreaseVacancySearchResultViews(string vacancyReference, int viewCount = 1);
    }
}
