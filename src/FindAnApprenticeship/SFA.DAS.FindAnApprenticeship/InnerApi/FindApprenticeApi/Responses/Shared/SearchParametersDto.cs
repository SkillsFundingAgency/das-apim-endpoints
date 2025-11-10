using System;
using System.Collections.Generic;
using System.Linq;
using SFA.DAS.SharedOuterApi.Domain;

namespace SFA.DAS.FindAnApprenticeship.InnerApi.FindApprenticeApi.Responses.Shared;

public record SearchParametersDto(
    string? SearchTerm,
    List<int>? SelectedRouteIds,
    int? Distance,
    bool DisabilityConfident,
    bool? ExcludeNational,
    List<int>? SelectedLevelIds,
    string? Location,
    string? Latitude,
    string? Longitude,
    List<ApprenticeshipTypes>? SelectedApprenticeshipTypes
)
{
    public virtual bool Equals(SearchParametersDto other)
    {
        if (other is null)
        {
            return false;
        }

        var thisRoutes = SelectedRouteIds ?? [];
        var otherRoutes = other.SelectedRouteIds ?? [];
        var routesEqual = thisRoutes.Count == otherRoutes.Count && !thisRoutes.Except(otherRoutes).Any(); 
        
        if (routesEqual is false)
        {
            return false;
        }
        
        var thisLevels = SelectedLevelIds ?? [];
        var otherLevels = other.SelectedLevelIds ?? [];
        var levelsEqual = thisLevels.Count == otherLevels.Count && !thisLevels.Except(otherLevels).Any();
        
        if (levelsEqual is false)
        {
            return false;
        }
        
        var thisAppTypes = SelectedApprenticeshipTypes ?? [];
        var otherAppTypes = other.SelectedApprenticeshipTypes ?? [];
        var appTypesEqual = thisAppTypes.Count == otherAppTypes.Count && !thisAppTypes.Except(otherAppTypes).Any();
        
        return
            appTypesEqual
            && SearchTerm == other.SearchTerm
            && Distance == other.Distance
            && DisabilityConfident == other.DisabilityConfident
            && ExcludeNational == other.ExcludeNational
            && Location == other.Location
            && Latitude == other.Latitude
            && Longitude == other.Longitude;
    }

    public override int GetHashCode()
    {
        var routesValue = string.Join(",", (SelectedRouteIds ?? []).Order());
        var levelsValue = string.Join(",", (SelectedLevelIds ?? []).Order());
        var appTypesValue = string.Join(",", (SelectedApprenticeshipTypes ?? []).Order());
        var hash = HashCode.Combine(routesValue, levelsValue, appTypesValue);
        
        return HashCode.Combine(SearchTerm, Distance, DisabilityConfident, Location, Latitude, Longitude, hash);
    }
}