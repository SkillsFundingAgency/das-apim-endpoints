using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Vacancies.InnerApi.Requests
{
    public class GetVacancyRequest : IGetApiRequest
    {
        private readonly string _vacancyReference;

        public GetVacancyRequest(string vacancyReference)
        {
            _vacancyReference = vacancyReference;
        }

        public string GetUrl => $"api/Vacancies/{_vacancyReference}";
        public string Version => "2.0";
    }
}