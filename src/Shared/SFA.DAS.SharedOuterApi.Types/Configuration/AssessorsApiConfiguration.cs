using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.SharedOuterApi.Types.Configuration;

public class AssessorsApiConfiguration : IInternalApiConfiguration
{
    public string Url { get; set; }
    public string Identifier { get; set; }
}