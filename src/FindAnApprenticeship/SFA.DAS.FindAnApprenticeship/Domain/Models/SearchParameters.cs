﻿using System.Collections.Generic;

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
    string? Longitude
);