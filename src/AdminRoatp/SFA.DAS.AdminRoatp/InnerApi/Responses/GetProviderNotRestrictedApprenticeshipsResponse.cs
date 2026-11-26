using SFA.DAS.AdminRoatp.InnerApi.Models;

namespace SFA.DAS.AdminRoatp.InnerApi.Responses;

public class GetProviderNotRestrictedApprenticeshipsResponse
{
    public List<NotRestrictedApprenticeshipModel> Courses { get; set; } = new List<NotRestrictedApprenticeshipModel>();
}
