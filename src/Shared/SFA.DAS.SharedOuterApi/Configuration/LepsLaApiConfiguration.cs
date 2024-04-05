using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.SharedOuterApi.Configuration
{
    public class LepsLaApiConfiguration : IInternalApiConfiguration
    {
        public string Url { get; set; }
        public string Identifier { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string KeyVaultIdentifier { get; set; }
        public string CertificateName { get; set; }
        public string Scope { get; set; }
    }
}
