using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.Aodp.Configuration
{
    [ExcludeFromCodeCoverage]
    public class AodpConfiguration
    {
        public List<NotificationTemplate> NotificationTemplates { get; set; }
        public string QfauReviewerEmailAddress { get; set; }
    }
}
