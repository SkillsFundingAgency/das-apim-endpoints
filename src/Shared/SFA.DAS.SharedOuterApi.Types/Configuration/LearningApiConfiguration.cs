using System.Diagnostics.CodeAnalysis;

using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.SharedOuterApi.Types.Configuration
{
    [ExcludeFromCodeCoverage]
    public class LearningApiConfiguration : IInternalApiConfiguration
    {
        public string Url { get; set; }
        public string Identifier { get; set; }
    }
}