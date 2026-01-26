using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.RecruitQa.InnerApi.Requests;

public class GetVacancyReviewsByVacancyReferenceRequest(long vacancyReference, string? status, bool includeNoStatus, List<string>? manualOutcome = null) : IGetApiRequest
{
    public string GetUrl => manualOutcome != null && manualOutcome.Any()
        ? $"api/vacancies/{vacancyReference}/Reviews?status={status}&manualOutcome={string.Join("&manualOutcome=", manualOutcome)}&includeNoStatus={includeNoStatus}"
        : $"api/vacancies/{vacancyReference}/Reviews?status={status}&includeNoStatus={includeNoStatus}";
}
