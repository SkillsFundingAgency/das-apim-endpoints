using System.Collections.Generic;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindAnApprenticeship.InnerApi.Requests
{
    public class GetVacanciesRequest : IGetApiRequest
    {
        private readonly double? _lat;
        private readonly double? _lon;
        private readonly string _routes;
        private readonly int? _distance;
        private readonly string _sort;

        public GetVacanciesRequest(double? lat, double? lon, List<string>? routes, int? distance, string sort)
        {
            _lat = lat;
            _lon = lon;
            _routes = routes != null ? string.Join("&routes=", routes) : "";
            _distance = distance;
            _sort = sort;
        }

        public string GetUrl => $"/api/vacancies?lat={_lat}&lon={_lon}&routes={_routes}&distanceInMiles={_distance}&sort={_sort}";
    }
}