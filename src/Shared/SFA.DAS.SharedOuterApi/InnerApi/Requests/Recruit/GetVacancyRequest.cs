using System;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.SharedOuterApi.InnerApi.Requests.Recruit;

public readonly struct GetVacancyRequest(Guid vacancyId) : IGetApiRequest
{
    public string GetUrl => $"api/vacancies/{vacancyId}";
}