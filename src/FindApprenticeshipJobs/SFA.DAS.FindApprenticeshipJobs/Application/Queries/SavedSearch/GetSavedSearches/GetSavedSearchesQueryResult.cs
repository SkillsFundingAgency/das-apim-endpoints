using SFA.DAS.FindApprenticeshipJobs.InnerApi.Responses;

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
        public string? UnSubscribeToken { get; set; }

        public static implicit operator SearchResult(GetSavedSearchesApiResponse.SavedSearch savedSearch)
        {
            return new SearchResult
            {
                Id= savedSearch.Id,
                UserId = savedSearch.UserReference,
                Distance = savedSearch.SearchParameters.Distance,
                SearchTerm = savedSearch.SearchParameters.SearchTerm,
                Location = savedSearch.SearchParameters.Location,
                DisabilityConfident = savedSearch.SearchParameters.DisabilityConfident,
                ExcludeNational = savedSearch.SearchParameters.ExcludeNational,
                UnSubscribeToken = savedSearch.UnSubscribeToken,
                Longitude = savedSearch.SearchParameters.Longitude,
                Latitude = savedSearch.SearchParameters.Latitude,
                SelectedLevelIds =savedSearch.SearchParameters.SelectedLevelIds,
                SelectedRouteIds =savedSearch.SearchParameters.SelectedRouteIds,
            };
        }
    }
}