using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.FindAnApprenticeship.InnerApi.RecruitApi.Requests
{
    public class GetClosedVacancyRequest(string vacancyReference) : IGetApiRequest
    {
        public string GetUrl => $"api/vacancies/{vacancyReference}/closed";
    }
}
