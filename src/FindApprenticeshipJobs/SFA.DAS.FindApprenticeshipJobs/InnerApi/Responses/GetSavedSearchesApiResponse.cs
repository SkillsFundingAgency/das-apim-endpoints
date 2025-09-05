using Newtonsoft.Json;
using SFA.DAS.SharedOuterApi.Domain;

namespace SFA.DAS.FindApprenticeshipJobs.InnerApi.Responses
{
    public record GetSavedSearchesApiResponse
    {
        [JsonProperty("savedSearches")]
        public List<SavedSearch> SavedSearches { get; set; } = [];

        [JsonProperty("totalCount")]
        public int TotalCount { get; set; }

        [JsonProperty("pageIndex")]
        public int PageIndex { get; set; }

        [JsonProperty("pageSize")]
        public int PageSize { get; set; }

        [JsonProperty("totalPages")]
        public int TotalPages { get; set; }

        public record SavedSearch
        {
            [JsonProperty("id")]
            public Guid Id { get; set; }

            [JsonProperty("userReference")]
            public Guid UserReference { get; set; }

            [JsonProperty("dateCreated")]
            public DateTime DateCreated { get; set; }

            [JsonProperty("lastRunDate")]
            public DateTime? LastRunDate { get; set; }

            [JsonProperty("emailLastSendDate")]
            public DateTime? EmailLastSendDate { get; set; }

            [JsonProperty("searchParameters")]
            public SearchParameters SearchParameters { get; set; } = null!;

            [JsonProperty("unSubscribeToken")] 
            public string? UnSubscribeToken { get; set; }
        }

        public record SearchParameters
        {
            [JsonProperty("selectedRouteIds")]
            public List<int>? SelectedRouteIds { get; set; } = [];

            [JsonProperty("selectedLevelIds")]
            public List<int>? SelectedLevelIds { get; set; } = [];

            [JsonProperty("latitude")]
            public string? Latitude { get; set; }

            [JsonProperty("longitude")]
            public string? Longitude { get; set; }

            [JsonProperty("distance")] 
            public decimal? Distance { get; set; } = null;

            [JsonProperty("searchTerm")]
            public string? SearchTerm { get; set; }

            [JsonProperty("location")]
            public string? Location { get; set; }
            
            [JsonProperty("disabilityConfident")]
            public bool DisabilityConfident { get; set; }

            [JsonProperty("excludeNational")]
            public bool? ExcludeNational { get; set; }
            
            [JsonProperty("selectedApprenticeshipTypes")]
            public List<ApprenticeshipTypes>? SelectedApprenticeshipTypes { get; set; }
        }
    }
}