using SFA.DAS.SharedOuterApi.Types.InnerApi.Responses.Roatp.Common;

namespace SFA.DAS.AdminRoatp.InnerApi.Requests;
public class PatchOrganisationModel
{
    public OrganisationStatus Status { get; set; }
    public int? RemovedReasonId { get; set; }
    public ProviderType ProviderType { get; set; }
    public int OrganisationTypeId { get; set; }
}
