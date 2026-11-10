using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.AdminRoatp.InnerApi.Requests;

public class GetStandardsRequest : IGetApiRequest
{
    public string GetUrl => "standards";
}
