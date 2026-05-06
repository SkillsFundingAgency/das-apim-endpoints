using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.RecruitJobs.InnerApi.Requests;

public class GetVacancyReviewsByVacancyReferenceRequest(long vacancyReference): IGetAllApiRequest
{
    public string GetAllUrl => $"/api/vacancies/{vacancyReference}/reviews";
}