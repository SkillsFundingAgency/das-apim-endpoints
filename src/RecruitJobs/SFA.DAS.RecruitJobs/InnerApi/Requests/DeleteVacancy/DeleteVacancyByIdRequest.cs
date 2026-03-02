using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.RecruitJobs.InnerApi.Requests.DeleteVacancy;

public record DeleteVacancyByIdRequest(Guid VacancyId) : IDeleteApiRequest
{
    public string DeleteUrl => $"api/vacancies/{VacancyId}";
}