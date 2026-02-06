using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.RecruitQa.InnerApi.Requests;

public record GetVacancyByIdRequest(Guid Id) : IGetApiRequest
{
    public string GetUrl => $"api/vacancies/{Id}";
}