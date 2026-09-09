using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.ToolsSupport.InnerApi.Requests;

public class GetEmployerAccountResourceByUrlRequest(string href) : IGetApiRequest
{
    public string GetUrl => href;
}
