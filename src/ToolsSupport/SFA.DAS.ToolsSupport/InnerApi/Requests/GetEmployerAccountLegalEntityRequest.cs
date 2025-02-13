using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.ToolsSupport.InnerApi.Requests;

public class GetEmployerAccountLegalEntityRequest(string href) : IGetApiRequest
{
    public string GetUrl => href;
}
