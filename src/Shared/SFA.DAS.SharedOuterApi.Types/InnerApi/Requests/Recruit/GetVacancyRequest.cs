using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.Recruit;

public readonly struct GetVacancyRequest(Guid vacancyId) : IGetApiRequest
{
    public string GetUrl => $"api/vacancies/{vacancyId}";
}