using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.DigitalCertificates.Configuration
{
    [ExcludeFromCodeCoverage]
    public class DigitalCertificatesConfiguration
    {
        public List<NotificationTemplate> NotificationTemplates { get; set; }
        public double MinMatch { get; set; }
    }
}
