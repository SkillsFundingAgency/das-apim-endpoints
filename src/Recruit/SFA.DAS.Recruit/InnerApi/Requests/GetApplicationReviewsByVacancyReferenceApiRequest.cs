using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.Recruit.InnerApi.Requests;

public sealed record GetApplicationReviewsByVacancyReferenceApiRequest(long VacancyReference) : IGetApiRequest
{
    public string GetUrl => $"api/vacancies/byRef/{VacancyReference}/applicationReviews";
}