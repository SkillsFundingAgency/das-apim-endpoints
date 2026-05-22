namespace SFA.DAS.ApprenticeAan.Application.Locations.Queries.GetLocationsBySearch;

public class GetLocationsBySearchQueryResult
{
    public IEnumerable<Location> Locations { get; set; } = new List<Location>();

    public class Location
    {
        public string Name { get; set; } = string.Empty;
    }
}