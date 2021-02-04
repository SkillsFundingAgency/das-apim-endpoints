using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.ApprenticeCommitments.Configuration
{
    public class ApprenticeLoginConfiguration : IInternalApiConfiguration
    {
        public string Url { get; set; }
        public string Identifier { get; set; }
        public string IdentityServerClientId { get; set; }
        public string CallbackUrl { get; set; }
        public string RedirectUrl { get; set; }
    }
}