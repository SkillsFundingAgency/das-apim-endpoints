using SFA.DAS.SharedOuterApi.Interfaces;
using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.SharedOuterApi.Configuration
{
    [ExcludeFromCodeCoverage]
    public class ShortlistApiConfiguration : IInternalApiConfiguration
    {
        public string Url { get; set; }
        public string Identifier { get; set; }
    }
}
