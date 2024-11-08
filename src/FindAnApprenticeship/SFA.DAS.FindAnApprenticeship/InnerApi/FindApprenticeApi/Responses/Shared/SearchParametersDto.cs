using System.Collections.Generic;

namespace SFA.DAS.FindAnApprenticeship.InnerApi.FindApprenticeApi.Responses.Shared;

public record SearchParametersDto(
    string? SearchTerm,
    List<int>? SelectedRouteIds,
    int? Distance,
    bool DisabilityConfident,
    List<int>? SelectedLevelIds,
    string? Location,
    string? Latitude,
    string? Longitude
);