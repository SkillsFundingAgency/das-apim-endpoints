using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Primitives;
using SFA.DAS.FindApprenticeshipJobs.Domain.Models;
using SFA.DAS.SharedOuterApi.Domain;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindApprenticeshipJobs.InnerApi.Requests
{
    public class GetVacanciesRequest(
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
        bool? excludeNational,
        IReadOnlyCollection<VacancyDataSource> additionalDataSources,
        IReadOnlyCollection<ApprenticeshipTypes>? apprenticeshipTypes) : IGetApiRequest
    {
        public string Version => "2.0";

        public string GetUrl
        {
            get
            {
                const string url = "/api/vacancies";
                var queryParameters = new Dictionary<string, StringValues>
                {
                    { "lat", lat is null ? null : $"{lat}" },
                    { "lon", lon is null ? null : $"{lon}" },
                    { "distanceInMiles", distance is null ? null : $"{distance}" },
                    { "sort", $"{sort}" },
                    { "pageNumber", pageNumber is null ? null : $"{pageNumber}" },
                    { "pageSize", pageSize is null ? null : $"{pageSize}" },
                    { "categories", categories?.ToArray() },
                    { "levels", levels?.Select(x => $"{x}").ToArray() },
                    { "searchTerm", searchTerm },
                    { "disabilityConfident", disabilityConfident.ToString() },
                    { "excludeNational", excludeNational is null ? null : excludeNational.ToString() },
                    { "additionalDataSources", additionalDataSources?.Select(x => $"{x}").ToArray() },
                    { "postedInLastNumberOfDays", "7" },
                    { "apprenticeshipTypes", apprenticeshipTypes?.Select(x => $"{x}").ToArray() },
                };
                
                return QueryHelpers.AddQueryString(url, queryParameters);
            }
        }
    }
}