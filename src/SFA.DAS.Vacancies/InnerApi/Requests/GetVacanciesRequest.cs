using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Vacancies.InnerApi.Requests
{
    public class GetVacanciesRequest : IGetApiRequest
    {
        private readonly int _pageNumber;
        private readonly int _pageSize;

        public GetVacanciesRequest(int pageNumber, int pageSize)
        {
            _pageNumber = pageNumber;
            _pageSize = pageSize;
        }
        public string GetUrl => $"api/Vacancies?{_pageNumber}&{_pageSize}";
    }
}
