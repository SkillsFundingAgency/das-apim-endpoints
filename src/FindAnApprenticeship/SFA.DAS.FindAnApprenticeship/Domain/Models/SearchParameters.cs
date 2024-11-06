using System.Collections.Generic;

namespace SFA.DAS.FindAnApprenticeship.Domain.Models;

public record SearchParameters(
    string? SearchTerm,
    List<string>? Categories,
    int? Distance,
    bool DisabilityConfident,
    List<string>? Levels,
    string? Location,
    string? Latitude,
    string? Longitude
);