using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.RecruitJobs.InnerApi.Requests.DeleteVacancy;

public record DeleteVacancyByIdRequest(Guid VacancyId) : IDeleteApiRequest
{
    public string DeleteUrl => $"api/vacancies/{VacancyId}";
}