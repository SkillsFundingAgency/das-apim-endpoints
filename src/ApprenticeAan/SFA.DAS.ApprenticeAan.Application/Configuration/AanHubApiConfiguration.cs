using System.Diagnostics.CodeAnalysis;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.ApprenticeAan.Api.Configuration
{
    [ExcludeFromCodeCoverage]
    public class AanHubApiConfiguration : IInternalApiConfiguration
    {
        public string Url { get; set; } = null!;
        public string Identifier { get; set; } = null!;
    }
}