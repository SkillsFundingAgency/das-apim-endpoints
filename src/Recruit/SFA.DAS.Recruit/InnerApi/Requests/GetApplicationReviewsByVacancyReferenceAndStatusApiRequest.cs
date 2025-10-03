using SFA.DAS.Recruit.Enums;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Recruit.InnerApi.Requests;
public record GetApplicationReviewsByVacancyReferenceAndStatusApiRequest(
    long VacancyReference,
    ApplicationReviewStatus Status,
    bool IncludeTemporaryStatus = false) : IGetApiRequest
{
    public string GetUrl => $"api/applicationReviews/{VacancyReference}/status/{Status}?includeTemporaryStatus={IncludeTemporaryStatus}";
}