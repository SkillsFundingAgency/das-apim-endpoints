using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.DigitalCertificates.Configuration
{
    [ExcludeFromCodeCoverage]
    public class PrivateBetaConfiguration
    {
        public bool WhitelistEnabled { get; set; }
        public List<long> AllowedUlns { get; set; } = [];
    }
}
