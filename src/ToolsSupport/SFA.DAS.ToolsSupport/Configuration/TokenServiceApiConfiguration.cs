using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.ToolsSupport.Configuration;

public class TokenServiceApiConfiguration: IInternalApiConfiguration
{
    public string Url { get; set; } = "";
    public string Identifier { get; set; } = "";
}
