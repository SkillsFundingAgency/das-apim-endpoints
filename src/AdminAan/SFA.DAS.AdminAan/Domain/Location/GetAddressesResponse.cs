namespace SFA.DAS.AdminAan.Domain.Location;

public class GetAddressesResponse
{
    public List<AddressListItem> Addresses { get; set; } = new();
}

public class AddressListItem
{
    public string Uprn { get; set; } = null!;
    public string? Organisation { get; set; }
    public string? Premises { get; set; }
    public string? Thoroughfare { get; set; }
    public string? Locality { get; set; }
    public string? PostTown { get; set; }
    public string? County { get; set; }
    public string? Postcode { get; set; }
    public string? AddressLine1 { get; set; }
    public string? AddressLine2 { get; set; }
    public string? AddressLine3 { get; set; }
    public double? Longitude { get; set; }
    public double? Latitude { get; set; }
    public double? Match { get; set; }
}
