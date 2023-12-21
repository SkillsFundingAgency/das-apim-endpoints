using System.Collections.Generic;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindAnApprenticeship.InnerApi.Requests
{
    public class GetVacanciesRequest : IGetApiRequest
    {
        private readonly double? _lat;
        private readonly double? _lon;
        private readonly int? _distance;
        private readonly int? _pageNumber;
        private readonly int? _pageSize;
        private readonly string _sort;
        private readonly string _categories;

        public GetVacanciesRequest(
            double? lat,
            double? lon,
            int? distance,
            string sort,
            int? pageNumber,
            int? pageSize,
            IReadOnlyCollection<string> categories)
        {
            _lat = lat;
            _lon = lon;
            _distance = distance;
            _sort = sort;
            _pageNumber = pageNumber;
            _pageSize = pageSize;
            _categories = categories != null ? string.Join("&categories=", categories) : string.Empty;
        }

        public string GetUrl => $"/api/vacancies?lat={_lat}&lon={_lon}&distanceInMiles={_distance}&sort={_sort}&pageNumber={_pageNumber}&pageSize={_pageSize}&categories={_categories}";

        public string Version => "2.0";
    }
}