using System;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Recruit.InnerApi.Recruit.Requests.Vacancies;

public record GetVacancyByIdRequest(Guid Id) : IGetApiRequest
{
    public string GetUrl => $"api/vacancies/{Id}";
}