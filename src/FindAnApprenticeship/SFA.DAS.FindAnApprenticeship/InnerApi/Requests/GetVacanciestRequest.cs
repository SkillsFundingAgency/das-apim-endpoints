using System.Collections.Generic;
using SFA.DAS.FindAnApprenticeship.Models;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindAnApprenticeship.InnerApi.Requests
{
    public class GetVacanciesRequest : IGetApiRequest
    {
        private readonly double? _lat;
        private readonly double? _lon;
        private readonly string _routes;
        private readonly int? _distance;
        private readonly int? _pageNumber;
        private readonly int? _pageSize;
        private readonly VacancySort _sort;

        public GetVacanciesRequest(
            double? lat,
            double? lon,
            List<string>? routes,
            int? distance,
            VacancySort sort,
            int? pageNumber,
            int? pageSize)
        {
            _lat = lat;
            _lon = lon;
            _routes = routes != null ? string.Join("&routes=", routes) : "";
            _distance = distance;
            _sort = sort;
            _pageNumber = pageNumber;
            _pageSize = pageSize;
        }

        public string GetUrl => $"/api/vacancies?lat={_lat}&lon={_lon}&routes={_routes}&distanceInMiles={_distance}&sort={_sort}&pageNumber={_pageNumber}&pageSize={_pageSize}";
    }
}