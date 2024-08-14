using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.SharedOuterApi.Configuration
{
    public class ApprenticeProgressApiConfiguration : IInternalApiConfiguration
    {
        public string Url { get; set; }
        public string Identifier { get; set; }
    }
}