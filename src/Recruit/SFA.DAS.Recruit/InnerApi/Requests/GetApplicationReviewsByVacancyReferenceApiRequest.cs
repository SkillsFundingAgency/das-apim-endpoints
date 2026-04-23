using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.Recruit.InnerApi.Requests
{
    public record GetApplicationReviewsByVacancyReferenceApiRequest(long VacancyReference) : IGetApiRequest
    {
        public string GetUrl => $"api/applicationReviews/{VacancyReference}";
    }
}