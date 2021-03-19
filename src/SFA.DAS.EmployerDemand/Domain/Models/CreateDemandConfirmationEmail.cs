using SFA.DAS.SharedOuterApi.Models;

namespace SFA.DAS.EmployerDemand.Domain.Models
{
    public class CreateDemandConfirmationEmail : SendEmailCommand
    {
        
        public CreateDemandConfirmationEmail(string recipientEmail, string recipientName, string standardName, int standardLevel, string location, int numberOfApprentices)
        {
            TemplateId = EmailTemplates.CreateDemandConfirmation;
            RecipientAddress = recipientEmail;
        }
    }
}