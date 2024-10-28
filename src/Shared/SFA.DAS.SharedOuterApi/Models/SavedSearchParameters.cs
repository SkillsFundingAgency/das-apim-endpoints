using System.Collections.Generic;

namespace SFA.DAS.SharedOuterApi.Models;

public record SavedSearchParameters(
    string? SearchTerm,
    LocationItem? Location,
    int? Distance,
    string? SortOrder,
    bool DisabilityConfident,
    List<string>? SelectedLevelIds,
    List<string>? SelectedRouteIds
);