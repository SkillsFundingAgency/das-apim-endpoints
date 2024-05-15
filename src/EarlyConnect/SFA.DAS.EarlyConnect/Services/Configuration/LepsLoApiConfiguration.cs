using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.EarlyConnect.Services.Configuration
{
    public class LepsLoApiConfiguration : IInternalApiConfiguration
    {
        public string Url { get; set; }
        public string Identifier { get; set; }
        public string apiversion { get; set; }
        public string SignedPermissions { get; set; }
        public string SignedVersion { get; set; }
        public string Signature { get; set; }
        public string ApiKey { get; set; }
    }
}
