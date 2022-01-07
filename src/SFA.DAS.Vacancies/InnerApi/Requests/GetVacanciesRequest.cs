using System.Web;
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
        private readonly int? _standardLarsCode;
        private readonly bool? _nationwideOnly;
        private readonly double? _lat;
        private readonly double? _lon;
        private readonly uint? _distanceInMiles;
        private readonly string _route;
        private readonly uint? _postedInLastNumberOfDays;
        private readonly string _sort;

        public GetVacanciesRequest(int pageNumber, int pageSize, string accountLegalEntityPublicHashedId = "", int? ukprn = null, string accountPublicHashedId = "", int? standardLarsCode = null, bool? nationwideOnly = null, double? lat = null, double? lon = null, uint? distanceInMiles = null, string route = "", uint? postedInLastNumberOfDays = null, string sort = "")
        {
            _pageNumber = pageNumber;
            _pageSize = pageSize;
            _accountLegalEntityPublicHashedId = HttpUtility.UrlEncode(accountLegalEntityPublicHashedId);
            _ukprn = ukprn;
            _accountPublicHashedId = accountPublicHashedId;
            _standardLarsCode = standardLarsCode;
            _nationwideOnly = nationwideOnly;
            _lat = lat;
            _lon = lon;
            _distanceInMiles = distanceInMiles;
            _route = HttpUtility.UrlEncode(route);
            _postedInLastNumberOfDays = postedInLastNumberOfDays;
            _sort = sort;
        }

        public string GetUrl => $"api/Vacancies?pageNumber={_pageNumber}&pageSize={_pageSize}&ukprn={_ukprn}&accountLegalEntityPublicHashedId={_accountLegalEntityPublicHashedId}&accountPublicHashedId={_accountPublicHashedId}&standardLarsCode={_standardLarsCode}&nationwideOnly={_nationwideOnly}&lat={_lat}&lon={_lon}&distanceInMiles={_distanceInMiles}&route={_route}&sort={_sort}&postedInLastNumberOfDays={_postedInLastNumberOfDays}";
    }
}
