namespace SFA.DAS.FindAnApprenticeship.Api.Telemetry
{
    public interface IMetrics
    {
        void IncreaseVacancyViews(string vacancyReference, int viewCount = 1);
    }
}
