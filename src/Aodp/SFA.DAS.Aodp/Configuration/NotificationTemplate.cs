using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.Aodp.Configuration
{
    [ExcludeFromCodeCoverage]
    public class NotificationTemplate
    {
        /// <summary>
        /// The template name for the configurated GOV.UK Notify email template
        /// </summary>
        public string TemplateName { get; set; }

        /// <summary>
        /// The template Id for the configurated GOV.UK Notify email template
        /// </summary>
        public Guid TemplateId { get; set; }
    }
}
