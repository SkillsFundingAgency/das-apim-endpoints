using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Recruit.InnerApi.Requests;
public record GetApplicationReviewsCountByVacancyReferenceApiRequest(long VacancyReference) : IGetApiRequest
{
    public string GetUrl => $"api/applicationReviews/{VacancyReference}/count";
}