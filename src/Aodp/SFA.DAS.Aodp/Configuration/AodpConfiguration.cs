using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.Aodp.Configuration
{
    [ExcludeFromCodeCoverage]
    public class AodpConfiguration
    {
        public List<NotificationTemplate> NotificationTemplates { get; set; } = new();
        public string QfauReviewerEmailAddress { get; set; } = string.Empty;
        public string QFASTBaseUrl { get; set; } = string.Empty;
    }
}
