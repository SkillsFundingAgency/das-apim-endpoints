using System;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Recruit.InnerApi.Recruit.Requests.Vacancies;

public record GetVacancyByReferenceRequest(long VacancyReference) : IGetApiRequest
{
    public string GetUrl => $"api/vacancies/by-reference/{VacancyReference}";
}