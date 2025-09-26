namespace SFA.DAS.FindApprenticeshipTraining.Application.Providers.GetProviderSummary;

public class ProviderAddressModel
{
    public string AddressLine1 { get; set; }
    public string AddressLine2 { get; set; }
    public string AddressLine3 { get; set; }
    public string AddressLine4 { get; set; }
    public string Town { get; set; }
    public string Postcode { get; set; }
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }
}
