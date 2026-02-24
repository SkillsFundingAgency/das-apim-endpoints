using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Aodp.Configuration;

public class AodpApiConfiguration : IInternalApiConfiguration
{
    public string Url { get; set; }
    public string Identifier { get; set; }
}