using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindApprenticeshipJobs.InnerApi.Requests
{
    public class GetLiveVacancyApiRequest : IGetApiRequest
    {
        private readonly long _vacancyReference;

        public GetLiveVacancyApiRequest(long vacancyReference)
        {
            _vacancyReference = vacancyReference;
        }

        public string GetUrl => $"api/livevacancies/{_vacancyReference}";
    }
}
