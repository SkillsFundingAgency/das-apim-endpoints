using SFA.DAS.AdminRoatp.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Responses.Roatp;

namespace SFA.DAS.AdminRoatp.Application.Queries.GetUkrlp;

public class GetUkrlpQueryResult
{
    public string? LegalName { get; set; }
    public string? TradingName { get; set; }
    public string? CharityNumber { get; set; }
    public string? CompanyNumber { get; set; }

    public static implicit operator GetUkrlpQueryResult(UkrlpProviderModel source) => new()
    {
        LegalName = source.LegalName,
        TradingName = source.TradingName,
        CharityNumber = source.VerificationDetails?.FirstOrDefault(v => string.Equals(v.VerificationAuthority, VerificationAuthority.CharityCommission, StringComparison.OrdinalIgnoreCase))?.VerificationId,
        CompanyNumber = source.VerificationDetails?.FirstOrDefault(v => string.Equals(v.VerificationAuthority, VerificationAuthority.CompaniesHouse, StringComparison.OrdinalIgnoreCase))?.VerificationId,
    };
}
