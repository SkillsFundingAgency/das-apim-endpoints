using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.FindApprenticeshipJobs.InnerApi.Requests;

public class GetClosedVacancyApiRequest(long vacancyReference) : IGetApiRequest
{
    public string GetUrl => $"api/vacancies/{vacancyReference}/closed";
}