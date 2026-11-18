using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.PAS.Infrastructure;

public class PasApiConfiguration : IInternalApiConfiguration
{
    public required string Url { get; set; }
    public required string Identifier { get; set; }
}
