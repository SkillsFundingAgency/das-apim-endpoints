using SFA.DAS.FindApprenticeshipJobs.Domain.Models;

namespace SFA.DAS.FindApprenticeshipJobs.Api.Models;

public class GetCandidateSavedVacanciesRequest
{
    public int? MaxApprenticeshipSearchResultsCount { get; set; }
    public VacancySort ApprenticeshipSearchResultsSortOrder { get; set; }
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public decimal? Distance { get; set; }
    public string? SearchTerm { get; set; }
    public string? Location { get; set; }
    public bool DisabilityConfident { get; set; }
    public string? Longitude { get; set; }
    public string? Latitude { get; set; }
    public List<int>? SelectedLevelIds { get; set; } = [];
    public List<int>? SelectedRouteIds { get; set; } = [];
    public string? UnSubscribeToken { get; set; }
    public DateTime LastRunDateFilter { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
}