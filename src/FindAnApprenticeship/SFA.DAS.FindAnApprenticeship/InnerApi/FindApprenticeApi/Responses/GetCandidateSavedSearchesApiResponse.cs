using System;
using System.Collections.Generic;

namespace SFA.DAS.FindAnApprenticeship.InnerApi.FindApprenticeApi.Responses;

public class GetCandidateSavedSearchesApiResponse
{
    public record SavedSearchResponse(
        Guid Id,
        Guid UserReference,
        DateTime DateCreated,
        DateTime? LastRunDate,
        DateTime? EmailLastSendDate,
        SearchParametersResponse SearchParameters
    );

    public record SearchParametersResponse(
        string? SearchTerm,
        List<int>? SelectedRouteIds,
        decimal? Distance,
        bool DisabilityConfident,
        List<int>? SelectedLevelIds,
        string? Location,
        string? Latitude,
        string? Longitude
    );
    
    public List<SavedSearchResponse> SavedSearches { get; set; }
}