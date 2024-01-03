using System.Collections.Generic;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindAnApprenticeship.InnerApi.Requests
{
    public class GetApprenticeshipCountRequest : IGetApiRequest
    {
        private readonly double? _lat;
        private readonly double? _lon;
        private readonly int? _distance;
        private readonly string? _whatSearchTerm;
        private readonly string _categories;

        public GetApprenticeshipCountRequest(
            double? lat,
            double? lon,
            int? distance,
            IReadOnlyCollection<string> categories,
            string whatSearchTerm)
        {
            _lat = lat;
            _lon = lon;
            _distance = distance;
            _whatSearchTerm = whatSearchTerm;
            _categories = categories != null ? string.Join("&categories=", categories) : string.Empty;
        }

        public string GetUrl => string.IsNullOrEmpty(_categories)
            ? $"/api/vacancies/count?lat={_lat}&lon={_lon}&distance={_distance}&searchTerm={_whatSearchTerm}"
            : $"/api/vacancies/count?lat={_lat}&lon={_lon}&distance={_distance}&categories={_categories}&searchTerm={_whatSearchTerm}";

        public string Version => "2.0";
    }
}