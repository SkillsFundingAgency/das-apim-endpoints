using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.Recruit;

public class GetVacancyReviewsByVacancyReferenceRequest(long vacancyReference): IGetAllApiRequest
{
    public string GetAllUrl => $"/api/vacancies/{vacancyReference}/reviews";
}