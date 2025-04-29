using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.EarlyConnect.Services.Configuration
{
    public class LepsLaApiConfiguration : IInternalApiConfiguration
    {
        public string Url { get; set; }
        public string Identifier { get; set; }
        public string KeyVaultIdentifier { get; set; }
        public string CertificateName { get; set; }
    }
}
