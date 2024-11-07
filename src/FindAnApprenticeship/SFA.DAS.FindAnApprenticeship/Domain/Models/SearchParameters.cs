using System.Collections.Generic;

namespace SFA.DAS.FindAnApprenticeship.Domain.Models;

public record SearchParameters(
    string? SearchTerm,
    List<int>? SelectedRouteIds,
    decimal? Distance,
    bool DisabilityConfident,
    List<int>? SelectedLevelIds,
    string? Location,
    string? Latitude,
    string? Longitude
);