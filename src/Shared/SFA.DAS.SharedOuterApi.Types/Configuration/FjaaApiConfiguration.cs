using System.Diagnostics.CodeAnalysis;

using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.SharedOuterApi.Types.Configuration
{
    [ExcludeFromCodeCoverage]
    public class FjaaApiConfiguration : IInternalApiConfiguration
    {
        public string Url { get; set; }
        public string Identifier { get; set; }
    }
}
