using System.Collections.Generic;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindAnApprenticeship.InnerApi.Requests
{
    public class GetApprenticeshipCountRequest : IGetApiRequest
    {
        private readonly double? _lat;
        private readonly double? _lon;
        private readonly int? _distance;
        private readonly string _categories;

        public GetApprenticeshipCountRequest(
            double? lat,
            double? lon,
            int? distance,
            IReadOnlyCollection<string> categories)
        {
            _lat = lat;
            _lon = lon;
            _distance = distance;
            _categories = categories != null ? string.Join("&categories=", categories) : string.Empty;
        }

        public string GetUrl => $"/api/vacancies/count?lat={_lat}&lon={_lon}&distance={_distance}&categories={_categories}";

        public string Version => "2.0";
    }
}