using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.ToolsSupport.Configuration;

public class HmrcApiConfiguration : IHmrcApiConfiguration
{
    public string Url { get; set; } = "";
}

public interface IHmrcApiConfiguration : IApiConfiguration
{
}
