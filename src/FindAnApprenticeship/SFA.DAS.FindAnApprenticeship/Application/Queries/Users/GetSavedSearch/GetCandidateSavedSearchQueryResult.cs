using System.Collections.Generic;
using System.Linq;
using SFA.DAS.FindAnApprenticeship.Domain.Models;
using SFA.DAS.FindAnApprenticeship.InnerApi.FindApprenticeApi.Responses;
using SFA.DAS.FindAnApprenticeship.InnerApi.FindApprenticeApi.Responses.Shared;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;

namespace SFA.DAS.FindAnApprenticeship.Application.Queries.Users.GetSavedSearch;

public record GetCandidateSavedSearchQueryResult(SavedSearch SavedSearch, List<GetRoutesListItem> Routes)
{
    public static GetCandidateSavedSearchQueryResult From(GetCandidateSavedSearchApiResponse source, GetRoutesListResponse routes)
    {
        return new GetCandidateSavedSearchQueryResult(MapSavedSearch(source.SavedSearch), routes.Routes.ToList());
    }

    private static SavedSearch MapSavedSearch(SavedSearchDto source)
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
                source.SearchParameters.ExcludeNational,
                source.SearchParameters.SelectedLevelIds,
                source.SearchParameters.Location,
                source.SearchParameters.Latitude,
                source.SearchParameters.Longitude,
                source.SearchParameters.SelectedApprenticeshipTypes
            )
        );
    }
}