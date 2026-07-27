namespace SFA.DAS.SharedOuterApi.Types.Models;

public class AccountLegalEntityItem
{
    public string AccountLegalEntityPublicHashedId { get; set; }
    public string Name { get; set; }
    public string AccountHashedId { get; set; }
    public long AccountId { get; set; }
    public long AccountLegalEntityId { get; set; }
    public List<EmployerProfileAddress> Addresses { get; set; }

    public record EmployerProfileAddress(int Id,
        string AddressLine1,
        string? AddressLine2,
        string? AddressLine3,
        string? AddressLine4,
        string Postcode,
        double? Latitude,
        double? Longitude);
}