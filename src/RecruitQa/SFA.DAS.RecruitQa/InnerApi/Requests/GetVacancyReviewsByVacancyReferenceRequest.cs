using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.RecruitQa.InnerApi.Requests;

public class GetVacancyReviewsByVacancyReferenceRequest(long vacancyReference, string status) : IGetApiRequest
{
    public string GetUrl => $"{vacancyReference}/VacancyReviews?status={status}";
}
