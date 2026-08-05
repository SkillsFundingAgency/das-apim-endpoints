using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.Recruit.InnerApi.Recruit.Requests.VacancyAnalytics;

public sealed record GetOneVacancyAnalyticsApiRequest(long VacancyReference) : IGetApiRequest
{
    public string GetUrl => $"api/vacancyAnalytics/{VacancyReference}";
}