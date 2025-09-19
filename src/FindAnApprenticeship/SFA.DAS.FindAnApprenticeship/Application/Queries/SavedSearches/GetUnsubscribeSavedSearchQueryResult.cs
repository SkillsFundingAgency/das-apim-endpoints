using System.Collections.Generic;
using System.Linq;
using SFA.DAS.FindAnApprenticeship.Domain.Models;
using SFA.DAS.FindAnApprenticeship.InnerApi.FindApprenticeApi.Responses;
using SFA.DAS.FindAnApprenticeship.InnerApi.FindApprenticeApi.Responses.Shared;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;

namespace SFA.DAS.FindAnApprenticeship.Application.Queries.SavedSearches;

public record GetUnsubscribeSavedSearchQueryResult(SavedSearch SavedSearch, IEnumerable<GetRoutesListItem> Routes)
{
    public static GetUnsubscribeSavedSearchQueryResult From(GetSavedSearchUnsubscribeApiResponse source, GetRoutesListResponse routes)
    {
        return new GetUnsubscribeSavedSearchQueryResult(MapSavedSearch(source.SavedSearch), routes.Routes);
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