using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.DigitalCertificates.Configuration
{
    [ExcludeFromCodeCoverage]
    public class DigitalCertificatesConfiguration
    {
        public List<NotificationTemplate> NotificationTemplates { get; set; }
        public double MinMatch { get; set; }
        public int MaxMasks { get; set; } = 5;
        public int StandardMaskCount { get; set; } = 3;
    }
}
