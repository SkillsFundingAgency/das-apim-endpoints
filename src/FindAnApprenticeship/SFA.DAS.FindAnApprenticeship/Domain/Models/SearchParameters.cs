using System.Collections.Generic;
using SFA.DAS.SharedOuterApi.Domain;

namespace SFA.DAS.FindAnApprenticeship.Domain.Models;

public record SearchParameters(
    string? SearchTerm,
    List<int>? SelectedRouteIds,
    int? Distance,
    bool DisabilityConfident,
    bool? ExcludeNational,
    List<int>? SelectedLevelIds,
    string? Location,
    string? Latitude,
    string? Longitude,
    List<ApprenticeshipTypes>? SelectedApprenticeshipTypes
);