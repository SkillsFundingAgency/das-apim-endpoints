using System;
using SFA.DAS.FindAnApprenticeship.Domain.Models;

namespace SFA.DAS.FindAnApprenticeship.InnerApi.FindApprenticeApi.Responses.Shared;

public record SavedSearchDto(
    Guid Id,
    Guid UserReference,
    DateTime DateCreated,
    DateTime? LastRunDate,
    DateTime? EmailLastSendDate,
    SearchParametersDto SearchParameters
);

public static class SavedSearchExtensions
{
    public static SavedSearch ToDomain(this SavedSearchDto source)
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