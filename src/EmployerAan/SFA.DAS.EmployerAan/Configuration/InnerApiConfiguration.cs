using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.EmployerAan.Configuration;

[ExcludeFromCodeCoverage]
public abstract class InnerApiConfiguration
{
    public string Url { get; set; } = null!;
    public string Identifier { get; set; } = null!;
}