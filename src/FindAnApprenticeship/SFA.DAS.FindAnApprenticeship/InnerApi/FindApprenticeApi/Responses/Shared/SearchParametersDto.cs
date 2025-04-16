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
        
        var routesEqual = SelectedRouteIds is not null && other.SelectedRouteIds is not null
            ? (SelectedRouteIds.Count == other.SelectedRouteIds.Count) && !SelectedRouteIds.Except(other.SelectedRouteIds).Any()
            : SelectedRouteIds == other.SelectedRouteIds;

        if (routesEqual is false)
        {
            return false;
        }
        
        var levelsEqual = SelectedLevelIds is not null && other.SelectedLevelIds is not null
            ? (SelectedLevelIds.Count == other.SelectedLevelIds.Count) && !SelectedLevelIds.Except(other.SelectedLevelIds).Any()
            : SelectedLevelIds == other.SelectedLevelIds;
        
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
        return HashCode.Combine(SearchTerm, SelectedRouteIds, Distance, DisabilityConfident, SelectedLevelIds, Location, Latitude, Longitude);
    }
}