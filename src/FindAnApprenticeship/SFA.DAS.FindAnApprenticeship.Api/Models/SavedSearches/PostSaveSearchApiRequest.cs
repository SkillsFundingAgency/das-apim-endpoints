using System.Collections.Generic;

namespace SFA.DAS.FindAnApprenticeship.Api.Models.SavedSearches;

public record PostSaveSearchApiRequest(
    bool DisabilityConfident,
    bool? ExcludeNational,
    int? Distance,
    string? Location,
    string? SearchTerm,
    List<int>? SelectedLevelIds,
    List<int>? SelectedRouteIds,
    string UnSubscribeToken
);