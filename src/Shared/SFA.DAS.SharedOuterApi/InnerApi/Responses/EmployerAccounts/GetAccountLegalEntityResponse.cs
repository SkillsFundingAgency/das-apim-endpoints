using System;

namespace SFA.DAS.SharedOuterApi.InnerApi.Responses.EmployerAccounts;
public class GetAccountLegalEntityResponse
{
    public string Code { get; set; }
    public string DasAccountId { get; set; }
    public DateTime? DateOfInception { get; set; }
    public string Address { get; set; }
    public long LegalEntityId { get; set; }
    public string Name { get; set; }
    public string PublicSectorDataSource { get; set; }
    public string Sector { get; set; }
    public string Source { get; set; }
    public short SourceNumeric { get; set; }
    public string Status { get; set; }
    public long AccountLegalEntityId { get; set; }
    public string AccountLegalEntityPublicHashedId { get; set; }
}
