using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.SharedOuterApi.Configuration
{
    [ExcludeFromCodeCoverage]
    public class LearningApiConfiguration
    {
        public string Url { get; set; }
        public string Identifier { get; set; }
    }
}