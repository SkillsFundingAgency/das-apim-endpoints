using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.ToolsSupport.InnerApi.Requests;

public class GetEmployerAccountResourceByUrlRequest(string href) : IGetApiRequest
{
    public string GetUrl => href;
}
