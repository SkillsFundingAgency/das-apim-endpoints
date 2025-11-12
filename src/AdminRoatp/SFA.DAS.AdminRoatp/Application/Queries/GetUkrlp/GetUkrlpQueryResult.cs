using SFA.DAS.SharedOuterApi.InnerApi.Responses.Roatp;

namespace SFA.DAS.AdminRoatp.Application.Queries.GetUkrlp;
public class GetUkrlpQueryResult
{
    private const string CompaniesHouse = "companies house";
    private const string CharityCommision = "charity commission";
    public string? LegalName { get; set; }
    public string? TradingName { get; set; }
    public string? CharityNumber { get; set; }
    public string? CompanyNumber { get; set; }

    public static implicit operator GetUkrlpQueryResult(ProviderDetails source) => new()
    {
        LegalName = source.ProviderName,
        TradingName = source.ProviderAliases?.FirstOrDefault()?.Alias,
        CharityNumber = source.VerificationDetails?.FirstOrDefault(v => v.VerificationAuthority.ToLower() == CharityCommision.ToLower())?.VerificationId,
        CompanyNumber = source.VerificationDetails?.FirstOrDefault(v => v.VerificationAuthority.ToLower() == CompaniesHouse.ToLower())?.VerificationId,
    };
}