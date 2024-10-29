namespace SFA.DAS.FindApprenticeshipJobs.InnerApi.Responses
{
    public record GetSavedSearchesApiResponse
    {
        public List<SavedSearch> SavedSearches { get; set; } = [];
        public int TotalCount { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }

        public record SavedSearch
        {
            public Guid Id { get; set; }
            public Guid UserReference { get; set; }
            public DateTime DateCreated { get; set; }
            public DateTime? LastRunDate { get; set; }
            public DateTime? EmailLastSendDate { get; set; }
            public SearchParameters SearchCriteriaParameters { get; set; } = null!;
        }

        public record SearchParameters
        {
            public List<string>? Categories { get; set; } = [];
            public List<string>? Levels { get; set; } = [];
            public string? Latitude { get; set; }
            public string? Longitude { get; set; }
            public int? Distance { get; set; }
            public string? SearchTerm { get; set; }
            public bool DisabilityConfident { get; set; }
        }
    }
}