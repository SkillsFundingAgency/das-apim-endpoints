using System.Collections.Generic;
using System.Linq;
using SFA.DAS.FindAnApprenticeship.Domain.Models;
using SFA.DAS.FindAnApprenticeship.InnerApi.FindApprenticeApi.Responses;

namespace SFA.DAS.FindAnApprenticeship.Application.Queries.Users.GetSavedSearches;

public record GetCandidateSavedSearchesQueryResult(List<SavedSearch> SavedSearches)
{
    public static GetCandidateSavedSearchesQueryResult From(GetCandidateSavedSearchesApiResponse source)
    {
        return new GetCandidateSavedSearchesQueryResult(
            source.SavedSearches.Select(MapSavedSearch).ToList());
    }

    private static SavedSearch MapSavedSearch(GetCandidateSavedSearchesApiResponse.SavedSearchResponse source)
    {
        return new SavedSearch(
            source.Id,
            source.UserReference,
            source.DateCreated,
            source.LastRunDate,
            source.EmailLastSendDate,
            new SearchParameters(
                source.SearchParameters.SearchTerm,
                source.SearchParameters.Categories,
                source.SearchParameters.Distance,
                source.SearchParameters.DisabilityConfident,
                source.SearchParameters.Levels,
                source.SearchParameters.Location,
                source.SearchParameters.Latitude,
                source.SearchParameters.Longitude
            )
        );
    }
}