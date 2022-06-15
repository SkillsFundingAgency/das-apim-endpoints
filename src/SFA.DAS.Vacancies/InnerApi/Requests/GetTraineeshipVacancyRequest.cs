using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Vacancies.InnerApi.Requests
{
    public class GetTraineeshipVacancyRequest : IGetApiRequest
    {
        private readonly string _vacancyReference;

        public GetTraineeshipVacancyRequest(string vacancyReference)
        {
            _vacancyReference = vacancyReference;
        }

        public string GetUrl => $"api/Vacancies/{_vacancyReference}";
    }
}