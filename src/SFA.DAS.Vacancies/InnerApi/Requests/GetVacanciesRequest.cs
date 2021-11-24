using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Vacancies.InnerApi.Requests
{
    public class GetVacanciesRequest : IGetApiRequest
    {
        private readonly int _pageNumber;
        private readonly int _pageSize;
        private readonly string _accountLegalEntityPublicHashedId;
        private readonly int? _ukprn;
        private readonly string _accountPublicHashedId;

        public GetVacanciesRequest(int pageNumber, int pageSize, string accountLegalEntityPublicHashedId = "", int? ukprn = null, string accountPublicHashedId = "")
        {
            _pageNumber = pageNumber;
            _pageSize = pageSize;
            _accountLegalEntityPublicHashedId = accountLegalEntityPublicHashedId;
            _ukprn = ukprn;
            _accountPublicHashedId = accountPublicHashedId;
        }
        public string GetUrl => $"api/Vacancies?pageNumber={_pageNumber}&pageSize={_pageSize}&ukprn={_ukprn}&accountLegalEntityPublicHashedId={_accountLegalEntityPublicHashedId}&accountPublicHashedId={_accountPublicHashedId}";
    }
}
