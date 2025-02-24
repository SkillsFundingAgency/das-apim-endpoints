using System.Collections.Generic;
using SFA.DAS.FindAnApprenticeship.Domain.Models;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindAnApprenticeship.InnerApi.Requests
{
    public class GetApprenticeshipCountRequest(double? lat,
    double? lon,
    int? distance,
    string? searchTerm,
    int? pageNumber,
    int? pageSize,
    IReadOnlyCollection<string> categories,
    IReadOnlyCollection<int> levels,
    WageType? wageType,
    bool disabilityConfident,
    IReadOnlyCollection<VacancyDataSource> dataSources,
    bool? excludeNational)
    : IGetApiRequest
    {
        private readonly string _categories = categories is { Count: > 0 } ? string.Join("&categories=", categories) : string.Empty;
        private readonly string _levels = levels is { Count: > 0 } ? string.Join("&levels=", levels) : string.Empty;
        private readonly string _dataSources = dataSources is { Count: > 0 } ? string.Join("&dataSources=", dataSources) : string.Empty;

        public string Version => "2.0";
        public string GetUrl => $"/api/vacancies/count?" +
                                $"lat={lat}" +
                                $"&lon={lon}" +
                                $"&distanceInMiles={distance}" +
                                $"&pageNumber={pageNumber}" +
                                $"&pageSize={pageSize}" +
                                $"&categories={_categories}" +
                                $"&levels={_levels}" +
                                $"&searchTerm={searchTerm}" +
                                $"&disabilityConfident={disabilityConfident}" +
                                $"&wageType={wageType}" +
                                $"&dataSources={_dataSources}" +
                                $"&excludeNational={excludeNational}";
    }
}