using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.RecruitJobs.InnerApi.Requests.ApplicationReviews;

public record GetManyByVacancyReferenceApiRequest(long VacancyReference) : IGetAllApiRequest
{
    public string GetAllUrl => $"/api/applicationReviews/{VacancyReference}";
}