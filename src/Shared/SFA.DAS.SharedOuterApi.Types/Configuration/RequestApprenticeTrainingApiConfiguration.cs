using System.Diagnostics.CodeAnalysis;

using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.SharedOuterApi.Types.Configuration
{
    [ExcludeFromCodeCoverage]
    public class RequestApprenticeTrainingApiConfiguration : IInternalApiConfiguration
    {
        public string Url { get; set; }
        public string Identifier { get; set; }
    }
}
