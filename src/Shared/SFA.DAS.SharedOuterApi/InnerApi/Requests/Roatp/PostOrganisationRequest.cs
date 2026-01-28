using SFA.DAS.SharedOuterApi.InnerApi.Responses.Roatp.Common;

namespace SFA.DAS.SharedOuterApi.InnerApi.Requests.Roatp;
public class PostOrganisationRequest
{
    public int Ukprn { get; set; }
    public required string LegalName { get; set; }
    public string TradingName { get; set; }
    public string CompanyNumber { get; set; }
    public string CharityNumber { get; set; }
    public ProviderType ProviderType { get; set; }
    public int? OrganisationTypeId { get; set; }
    public required string RequestingUserId { get; set; }
}
