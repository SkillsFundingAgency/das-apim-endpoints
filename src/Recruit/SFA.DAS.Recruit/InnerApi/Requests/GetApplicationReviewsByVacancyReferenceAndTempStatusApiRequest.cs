using SFA.DAS.Recruit.Enums;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Recruit.InnerApi.Requests;
public record GetApplicationReviewsByVacancyReferenceAndTempStatusApiRequest(
    long VacancyReference,
    ApplicationReviewStatus Status) : IGetApiRequest
{
    public string GetUrl => $"api/applicationReviews/{VacancyReference}/temp-status/{Status}";
}