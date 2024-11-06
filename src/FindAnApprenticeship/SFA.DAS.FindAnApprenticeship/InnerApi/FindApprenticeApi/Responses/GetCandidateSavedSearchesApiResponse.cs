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
        List<string>? Categories,
        int? Distance,
        bool DisabilityConfident,
        List<string>? Levels,
        string? Location,
        string? Latitude,
        string? Longitude
    );
    
    public List<SavedSearchResponse> SavedSearches { get; set; }
}