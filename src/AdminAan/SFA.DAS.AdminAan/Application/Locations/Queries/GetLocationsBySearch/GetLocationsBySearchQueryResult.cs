﻿namespace SFA.DAS.AdminAan.Application.Locations.Queries.GetLocationsBySearch;

public class GetLocationsBySearchQueryResult
{
    public IEnumerable<Location> Locations { get; set; }

    public class Location
    {
        public string Name { get; set; }
    }
}