using System.Collections.Generic;
using System.Linq;
using SFA.DAS.FindAnApprenticeship.Domain.Models;
using SFA.DAS.FindAnApprenticeship.InnerApi.FindApprenticeApi.Responses;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;

namespace SFA.DAS.FindAnApprenticeship.Application.Queries.Users.GetSavedSearches;

public record GetCandidateSavedSearchesQueryResult(List<SavedSearch> SavedSearches, List<GetRoutesListItem> Routes)
{
    public static GetCandidateSavedSearchesQueryResult From(GetCandidateSavedSearchesApiResponse source, GetRoutesListResponse routes)
    {
        return new GetCandidateSavedSearchesQueryResult(source.SavedSearches.Select(MapSavedSearch).ToList(), routes.Routes.ToList());
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