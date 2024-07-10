namespace SFA.DAS.ApprenticeAan.Application.Infrastructure;

public class GetAddressesListItem
{
    public string Uprn { get; set; } = null!;
    public string Organisation { get; set; } = null!;
    public string Premises { get; set; } = null!;
    public string Thoroughfare { get; set; } = null!;
    public string Locality { get; set; } = null!;
    public string PostTown { get; set; } = null!;
    public string County { get; set; } = null!;
    public string Postcode { get; set; } = null!;
    public string AddressLine1 { get; set; } = null!;
    public string AddressLine2 { get; set; } = null!;
    public string AddressLine3 { get; set; } = null!;
    public double? Longitude { get; set; }
    public double? Latitude { get; set; }
    public double? Match { get; set; }
}

