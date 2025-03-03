using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindApprenticeshipJobs.InnerApi.Requests
{
    public class GetVacancyRequest(string vacancyReference) : IGetApiRequest
    {
        public string GetUrl => $"/api/vacancies/{vacancyReference}";
        public string Version => "2.0";
    }
}