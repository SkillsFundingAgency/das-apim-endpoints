using SFA.DAS.SharedOuterApi.Interfaces;
using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.AANApprentice.Api.Configuration;

[ExcludeFromCodeCoverage]
public class AanHubApiConfiguration : IInternalApiConfiguration
{
    public string Url { get; set; } = null!;
    public string Identifier { get; set; } = null!;
}
