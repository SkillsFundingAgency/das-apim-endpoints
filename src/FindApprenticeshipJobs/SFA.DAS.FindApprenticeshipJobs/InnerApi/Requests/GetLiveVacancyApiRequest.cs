using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindApprenticeshipJobs.InnerApi.Requests;

public class GetLiveVacancyApiRequest(long vacancyReference) : IGetApiRequest
{
    public string GetUrl => $"api/vacancies/{vacancyReference}/live";
}