using SFA.DAS.ApprenticeAan.Application.Infrastructure;

namespace SFA.DAS.ApprenticeAan.Application.Locations.Queries.GetAddresses;

public class AddressItem
{
    public string Uprn { get; set; } = null!;
    public string? OrganisationName { get; set; }
    public string? Town { get; set; }
    public string? County { get; set; }
    public string? Postcode { get; set; }
    public string? AddressLine1 { get; set; }
    public string? AddressLine2 { get; set; }
    public double? Longitude { get; set; }
    public double? Latitude { get; set; }

    public static implicit operator AddressItem(GetAddressesListItem source) =>
        new()
        {
            Uprn = source.Uprn,
            OrganisationName = source.Organisation,
            Town = source.PostTown,
            County = source.County,
            Postcode = source.Postcode,
            AddressLine1 = source.AddressLine1,
            AddressLine2 = source.AddressLine2,
            Longitude = source.Longitude,
            Latitude = source.Latitude
        };
}