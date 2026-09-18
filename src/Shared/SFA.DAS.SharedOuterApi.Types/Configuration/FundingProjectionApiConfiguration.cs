using SFA.DAS.Apim.Shared.Interfaces;
using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.SharedOuterApi.Types.Configuration;

[ExcludeFromCodeCoverage]
public sealed record FundingProjectionApiConfiguration : IInternalApiConfiguration
{
    public required string Url { get; set; }
    public required string Identifier { get; set; }
}