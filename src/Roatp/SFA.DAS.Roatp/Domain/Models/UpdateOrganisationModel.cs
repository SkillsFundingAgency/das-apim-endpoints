using SFA.DAS.SharedOuterApi.Types.InnerApi.Responses.Roatp.Common;

namespace SFA.DAS.Roatp.Domain.Models;

public class UpdateOrganisationModel
{
    public int? OrganisationTypeId { get; set; }
    public string LegalName { get; set; }
    public string TradingName { get; set; }
    public string CharityNumber { get; set; }
    public string CompanyNumber { get; set; }
    public string RequestingUserId { get; set; }
    public ProviderType ProviderType { get; set; }
}
