using SFA.DAS.Apim.Shared.Interfaces;
using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.SharedOuterApi.Types.Configuration;

[ExcludeFromCodeCoverage]
public class FinanceApiConfiguration : IInternalApiConfiguration
{
    public string Url { get; set; }
    public string Identifier { get; set; }
}