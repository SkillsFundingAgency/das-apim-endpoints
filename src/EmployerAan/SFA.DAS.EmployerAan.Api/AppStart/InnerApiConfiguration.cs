using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.EmployerAan.Api.AppStart;

[ExcludeFromCodeCoverage]
public class InnerApiConfiguration
{
    public string Url { get; set; } = null!;
    public string Identifier { get; set; } = null!;
}
