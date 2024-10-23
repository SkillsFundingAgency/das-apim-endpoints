using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.SharedOuterApi.Configuration
{
    public class LearnerDataApiConfiguration : IApiConfiguration
    {
        public string Url { get; set; }
        public LearnerDataTokenProviderSettings TokenSettings { get; set; }
    }

    public class LearnerDataTokenProviderSettings
    {
        public string Scope { get; set; }
        public string ClientId { get; set; }
        public string Tenant { get; set; }
        public string ClientSecret { get; set; }
    }
}
