using SFA.DAS.Apim.Shared.Interfaces;
using SFA.DAS.SharedOuterApi.Types.Domain.Recruit.Reviews;

namespace SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.Recruit;

public class PutVacancyReviewRequest(Guid vacancyReviewId, VacancyReview vacancyReview) : IPutApiRequest
{
    public string PutUrl => $"api/vacancyreviews/{vacancyReviewId}";
    public object Data { get; set; } = vacancyReview;
}