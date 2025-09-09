using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Primitives;
using SFA.DAS.FindAnApprenticeship.Domain.Models;
using SFA.DAS.SharedOuterApi.Domain;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindAnApprenticeship.InnerApi.Requests;

public class GetApprenticeshipCountRequest(
    double? lat,
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
    bool? excludeNational,
    IReadOnlyCollection<ApprenticeshipTypes> apprenticeshipTypes)
    : IGetApiRequest
{
    private readonly string _categories = categories is { Count: > 0 } ? string.Join("&categories=", categories) : string.Empty;
    private readonly string _levels = levels is { Count: > 0 } ? string.Join("&levels=", levels) : string.Empty;
    private readonly string _dataSources = dataSources is { Count: > 0 } ? string.Join("&dataSources=", dataSources) : string.Empty;

    public string Version => "2.0";

    public string GetUrl
    {
        get
        {
            var url = $"/api/vacancies/count?" +
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

            url = QueryHelpers.AddQueryString(url,
            [
                new KeyValuePair<string, StringValues>("apprenticeshipTypes",
                    apprenticeshipTypes?.Select(x => $"{x}").ToArray())
            ]);

            return url;
        }
    }
}