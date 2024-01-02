using System.Collections.Generic;
using SFA.DAS.FindAnApprenticeship.Models;
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
        private readonly string _categories;
        private readonly string? _whatSearchTerm;
        private readonly VacancySort _sort;

        public GetVacanciesRequest(
            double? lat,
            double? lon,
            int? distance,
            string? whatSearchTerm,
            int? pageNumber,
            int? pageSize,
            IReadOnlyCollection<string> categories,
            VacancySort sort)
        {
            _lat = lat;
            _lon = lon;
            _distance = distance;
            _sort = sort;
            _pageNumber = pageNumber;
            _pageSize = pageSize;
            _categories = categories is {Count: > 0} ? string.Join("&categories=", categories) : string.Empty;
            _whatSearchTerm = whatSearchTerm;
        }


        public string Version => "2.0";
        public string GetUrl => string.IsNullOrEmpty(_categories) 
            ? $"/api/vacancies?lat={_lat}&lon={_lon}&distanceInMiles={_distance}&sort={_sort}&pageNumber={_pageNumber}&pageSize={_pageSize}&searchTerm={_whatSearchTerm}" 
            : $"/api/vacancies?lat={_lat}&lon={_lon}&distanceInMiles={_distance}&sort={_sort}&pageNumber={_pageNumber}&pageSize={_pageSize}&categories={_categories}&searchTerm={_whatSearchTerm}";
    }
}