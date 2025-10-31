using System.Collections.Generic;

namespace SFA.DAS.RecruitJobs.Api.Models;

public class NotificationEmail
{
    public required Guid TemplateId { get; set; }
    public required string RecipientAddress { get; set; }
    public required Dictionary<string, string> Tokens { get; set; } = [];
    public required IEnumerable<long> SourceIds { get; set; } = [];
}