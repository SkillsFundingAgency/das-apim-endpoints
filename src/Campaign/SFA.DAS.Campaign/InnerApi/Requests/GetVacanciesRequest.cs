using System.Collections.Generic;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Campaign.InnerApi.Requests
{
    public class GetVacanciesRequest : IGetApiRequest
    {
        private readonly double? _lat;
        private readonly double? _lon;
        private readonly uint? _distance;
        private readonly int? _pageNumber;
        private readonly int? _pageSize;
        private readonly string _categories;


        public GetVacanciesRequest(
            double lat,
            double lon,
            uint distance,
            int pageNumber,
            int pageSize,
            string categories
            )
        {
            _lat = lat;
            _lon = lon;
            _distance = distance;
            _pageNumber = pageNumber;
            _pageSize = pageSize;
            _categories = categories;

        }


        public string Version => "2.0";
        public string GetUrl => $"/api/vacancies?lat={_lat}&lon={_lon}&distanceInMiles={_distance}&pageNumber={_pageNumber}&pageSize={_pageSize}&categories={_categories}";
    }
}