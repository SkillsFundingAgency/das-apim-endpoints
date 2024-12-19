using System.Web;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.FindApprenticeshipJobs.Domain.Models;

namespace SFA.DAS.FindApprenticeshipJobs.InnerApi.Requests
{
    public class GetVacanciesRequest : IGetApiRequest
    {
        private readonly double? _lat;
        private readonly double? _lon;
        private readonly decimal? _distance;
        private readonly int? _pageNumber;
        private readonly int? _pageSize;
        private readonly string _categories;
        private readonly string _levels;
        private readonly string _searchTerm;
        private readonly VacancySort _sort;
        private readonly bool _disabilityConfident;
        private readonly string _additionalDataSources;

        public GetVacanciesRequest(
            double? lat,
            double? lon,
            decimal? distance,
            string? searchTerm,
            int? pageNumber,
            int? pageSize,
            IReadOnlyCollection<string> categories,
            IReadOnlyCollection<int>? levels,
            VacancySort sort,
            bool disabilityConfident,
            IReadOnlyCollection<VacancyDataSource> additionalDataSources)
        {
            _lat = lat;
            _lon = lon;
            _distance = distance;
            _sort = sort;
            _pageNumber = pageNumber;
            _pageSize = pageSize;
            _categories = categories is { Count: > 0 } ? string.Join("&categories=", categories) : string.Empty;
            _levels = levels is { Count: > 0 } ? string.Join("&levels=", levels) : string.Empty;
            _searchTerm = searchTerm;
            _disabilityConfident = disabilityConfident;
            _additionalDataSources = additionalDataSources is { Count: > 0 } ? string.Join("&additionalDataSources=", additionalDataSources) : string.Empty;
        }

        public string Version => "2.0";
        public string GetUrl => $"/api/vacancies?lat={_lat}&lon={_lon}&distanceInMiles={_distance}&sort={_sort}&pageNumber={_pageNumber}&pageSize={_pageSize}&categories={_categories}&levels={_levels}&searchTerm={_searchTerm}&disabilityConfident={_disabilityConfident}&additionalDataSources={_additionalDataSources}&postedInLastNumberOfDays=7";
    }
}
public enum VacancyDataSource
{
    Raa,
    Nhs,
}