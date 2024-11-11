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
        return new GetCandidateSavedSearchesQueryResult(source.SavedSearches.Select(c=>c.MapSavedSearch()).ToList(), routes.Routes.ToList());
    }
}