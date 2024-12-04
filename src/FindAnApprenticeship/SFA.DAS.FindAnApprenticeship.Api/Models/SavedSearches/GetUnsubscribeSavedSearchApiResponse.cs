using System.Collections.Generic;
using System.Linq;
using SFA.DAS.FindAnApprenticeship.Application.Queries.SavedSearches;
using SFA.DAS.FindAnApprenticeship.Domain.Models;

namespace SFA.DAS.FindAnApprenticeship.Api.Models.SavedSearches;

public class GetUnsubscribeSavedSearchApiResponse
{
    public List<RouteApiResponse> Routes { get; init; }
    public SavedSearch SavedSearch { get; set; }

    public static implicit operator GetUnsubscribeSavedSearchApiResponse(GetUnsubscribeSavedSearchQueryResult source)
    {
        return new GetUnsubscribeSavedSearchApiResponse
        {
            Routes = source.Routes.Select(c => (RouteApiResponse)c).ToList(),
            SavedSearch = source.SavedSearch
        };
    }
}