using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindApprenticeshipJobs.InnerApi.Requests;

public class GetClosedVacancyApiRequest(long vacancyReference) : IGetApiRequest
{
    public string GetUrl => $"api/vacancies/{vacancyReference}/closed";
}