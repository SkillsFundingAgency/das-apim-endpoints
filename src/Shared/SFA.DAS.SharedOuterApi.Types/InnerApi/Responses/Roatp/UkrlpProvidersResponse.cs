namespace SFA.DAS.SharedOuterApi.Types.InnerApi.Responses.Roatp;

public record UkrlpProvidersResponse(IEnumerable<UkrlpProviderModel> Providers);

public class UkrlpProviderModel
{
    public int Ukprn { get; set; }
    public string LegalName { get; set; }
    public string TradingName { get; set; }
    public ProviderContactModel ContactDetails { get; set; }
    public Address Address { get; set; }
    public IEnumerable<VerificationInfo> VerificationDetails { get; set; }
}

public record ProviderContactModel(string Title, string FirstName, string LastName, string Email, string Telephone, string Website);

public record Address(string Address1, string Address2, string Address3, string Address4, string Town, string County, string PostCode);

public record VerificationInfo(string VerificationAuthority, string VerificationId, bool PrimaryVerificationSource);
