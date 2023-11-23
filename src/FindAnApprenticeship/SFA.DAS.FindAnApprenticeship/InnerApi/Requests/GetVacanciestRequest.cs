using System.Collections.Generic;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindAnApprenticeship.InnerApi.Requests
{
    public class GetVacanciesRequest : IGetApiRequest
    {
        private readonly double? _lat;
        private readonly double? _lon;
        private readonly List<string>? _routes;
        private readonly int? _distance;

        public GetVacanciesRequest(double? lat, double? lon, List<string>? routes, int? distance)
        {
            _lat = lat;
            _lon = lon;
            _routes = routes;
            _distance = distance;
        }

        public string GetUrl => $"/api/vacancies?lat={_lat}&lon={_lon}&routes={_routes}&distance={_distance}";
    }
}