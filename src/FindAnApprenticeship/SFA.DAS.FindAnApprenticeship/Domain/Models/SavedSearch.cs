using System;
using SFA.DAS.FindAnApprenticeship.InnerApi.FindApprenticeApi.Responses;

namespace SFA.DAS.FindAnApprenticeship.Domain.Models;

public record SavedSearch(
    Guid Id,
    Guid UserReference,
    DateTime DateCreated,
    DateTime? LastRunDate,
    DateTime? EmailLastSendDate,
    SearchParameters SearchParameters
);

public static class SavedSearchExtensions
{
    public static SavedSearch MapSavedSearch(this GetCandidateSavedSearchesApiResponse.SavedSearchResponse source)
    {
        return new SavedSearch(
            source.Id,
            source.UserReference,
            source.DateCreated,
            source.LastRunDate,
            source.EmailLastSendDate,
            new SearchParameters(
                source.SearchParameters.SearchTerm,
                source.SearchParameters.SelectedRouteIds,
                source.SearchParameters.Distance,
                source.SearchParameters.DisabilityConfident,
                source.SearchParameters.SelectedLevelIds,
                source.SearchParameters.Location,
                source.SearchParameters.Latitude,
                source.SearchParameters.Longitude
            )
        );
    }
}
