using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.AdminAan.Configuration;

[ExcludeFromCodeCoverage]
public class ReferenceDataApiConfiguration
{
    public string ApiBaseUrl { get; set; } = null!;
    public string IdentifierUri { get; set; } = null!;
}
