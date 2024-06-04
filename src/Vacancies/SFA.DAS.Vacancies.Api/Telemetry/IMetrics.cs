namespace SFA.DAS.Vacancies.Api.Telemetry
{
    public interface IMetrics
    {
        void IncreaseVacancyViews(string vacancyReference, int viewCount = 1);
    }
}
