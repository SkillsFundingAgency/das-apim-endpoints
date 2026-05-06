using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.RecruitJobs.InnerApi.Requests.VacancyAnalytics;

public sealed record GetOneVacancyAnalyticsApiRequest(long VacancyReference) : IGetApiRequest
{
    public string GetUrl => $"api/vacancyAnalytics/{VacancyReference}";
}