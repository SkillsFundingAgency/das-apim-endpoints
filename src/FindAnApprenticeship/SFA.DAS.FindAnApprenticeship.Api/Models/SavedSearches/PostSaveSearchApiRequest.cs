using System.Collections.Generic;
using SFA.DAS.SharedOuterApi.Domain;

namespace SFA.DAS.FindAnApprenticeship.Api.Models.SavedSearches;

public record PostSaveSearchApiRequest(
    bool DisabilityConfident,
    bool? ExcludeNational,
    int? Distance,
    string? Location,
    string? SearchTerm,
    List<int>? SelectedLevelIds,
    List<int>? SelectedRouteIds,
    string UnSubscribeToken,
    List<ApprenticeshipTypes>? ApprenticeshipTypes
);