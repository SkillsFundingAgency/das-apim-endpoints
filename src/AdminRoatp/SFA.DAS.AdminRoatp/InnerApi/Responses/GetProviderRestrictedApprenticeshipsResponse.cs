using SFA.DAS.AdminRoatp.InnerApi.Models;

namespace SFA.DAS.AdminRoatp.InnerApi.Responses;

public class GetProviderRestrictedApprenticeshipsResponse
{
    public List<RestrictedApprenticeshipModel> Courses { get; set; } = new List<RestrictedApprenticeshipModel>();
}
