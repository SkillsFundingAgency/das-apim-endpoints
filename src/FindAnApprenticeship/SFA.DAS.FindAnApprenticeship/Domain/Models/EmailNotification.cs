using System.Collections.Generic;

namespace SFA.DAS.FindAnApprenticeship.Domain.Models
{
    public record EmailNotification(string TemplateId, string RecipientAddress, Dictionary<string, string> Tokens);
}
