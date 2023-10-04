using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.AdminAan.Configuration;

[ExcludeFromCodeCoverage]
public class ReferenceDataApi
{
    public string ApiBaseUrl { get; set; } = null!;
    public string IdentifierUri { get; set; } = null!;
}
