using System;
using System.Collections.Generic;
using System.Linq;

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
    string? Longitude
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
        
        return
            levelsEqual
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
        return HashCode.Combine(SearchTerm, routesValue, Distance, DisabilityConfident, levelsValue, Location, Latitude, Longitude);
    }
}