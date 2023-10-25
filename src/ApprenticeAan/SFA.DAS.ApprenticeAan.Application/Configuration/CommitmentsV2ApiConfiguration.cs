using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.ApprenticeAan.Configuration;

[ExcludeFromCodeCoverage]
public class CommitmentsV2ApiConfiguration
{
    public string Url { get; set; } = null!;
    public string Identifier { get; set; } = null!;
}
