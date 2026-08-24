using SFA.DAS.AdminRoatp.InnerApi.Models;

namespace SFA.DAS.AdminRoatp.InnerApi.Responses;

public class GetAllStandardsResponse
{
    public List<StandardModel> Standards { get; set; } = new List<StandardModel>();
}
