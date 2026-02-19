using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.RecruitQa.InnerApi.Requests;

public class PutCreateVacancyReviewRequest(Guid id, VacancyReviewDto data) : IPutApiRequest
{
    public string PutUrl => $"api/vacancyreviews/{id}";
    public object Data { get; set; } = data;
}