using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.SharedOuterApi.Configuration
{
    public class LearnerDataApiConfiguration : IAccessTokenApiConfiguration
    {
        public string Url { get; set; }
        public AccessTokenProviderApiConfiguration TokenSettings { get; set; }
    }
}
