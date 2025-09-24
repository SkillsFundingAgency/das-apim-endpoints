using SFA.DAS.FindApprenticeshipJobs.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Domain;

namespace SFA.DAS.FindApprenticeshipJobs.Application.Queries.SavedSearch.GetSavedSearches;

public record GetSavedSearchesQueryResult
{
    public List<SearchResult> SavedSearchResults { get; set; } = null!;
    public int TotalCount { get; set; }
    public int PageIndex { get; set; }
    public int PageSize { get; set; }
    public int TotalPages { get; set; }
    public DateTime LastRunDateFilter { get; set; }

    public class SearchResult
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public decimal? Distance { get; set; }
        public string? SearchTerm { get; set; }
        public string? Location { get; set; }
        public bool DisabilityConfident { get; set; }
        public bool? ExcludeNational { get; set; }
        public string? Longitude { get; set; }
        public string? Latitude { get; set; }
        public List<int>? SelectedLevelIds { get; set; } = [];
        public List<int>? SelectedRouteIds { get; set; } = [];
        public List<ApprenticeshipTypes>? SelectedApprenticeshipTypes { get; set; } = [];
        public string? UnSubscribeToken { get; set; }

        public static implicit operator SearchResult(GetSavedSearchesApiResponse.SavedSearch savedSearch)
        {
            return new SearchResult
            {
                DisabilityConfident = savedSearch.SearchParameters.DisabilityConfident,
                Distance = savedSearch.SearchParameters.Distance,
                ExcludeNational = savedSearch.SearchParameters.ExcludeNational,
                Id= savedSearch.Id,
                Latitude = savedSearch.SearchParameters.Latitude,
                Location = savedSearch.SearchParameters.Location,
                Longitude = savedSearch.SearchParameters.Longitude,
                SearchTerm = savedSearch.SearchParameters.SearchTerm,
                SelectedApprenticeshipTypes = savedSearch.SearchParameters.SelectedApprenticeshipTypes,
                SelectedLevelIds =savedSearch.SearchParameters.SelectedLevelIds,
                SelectedRouteIds =savedSearch.SearchParameters.SelectedRouteIds,
                UnSubscribeToken = savedSearch.UnSubscribeToken,
                UserId = savedSearch.UserReference,
            };
        }
    }
}