using System.Collections.Generic;

namespace SFA.DAS.RecruitJobs.InnerApi.Responses.DelayedNotifications;

public class GetDelayedNotificationsByDateResponse
{
    public List<NotificationEmail> Emails { get; set; }
}

public class NotificationEmail
{
    public required Guid TemplateId { get; set; }
    public required string RecipientAddress { get; set; }
    public required Dictionary<string, string> Tokens { get; set; } = [];
    public required IEnumerable<long> SourceIds { get; set; } = [];
}