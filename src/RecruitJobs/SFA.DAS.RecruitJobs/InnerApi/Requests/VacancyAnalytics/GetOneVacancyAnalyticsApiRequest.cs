using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.RecruitJobs.InnerApi.Requests.VacancyAnalytics;

public sealed record GetOneVacancyAnalyticsApiRequest(long VacancyReference) : IGetApiRequest
{
    public string GetUrl => $"api/vacancyAnalytics/{VacancyReference}";
}