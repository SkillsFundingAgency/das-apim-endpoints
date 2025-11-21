namespace SFA.DAS.Aodp.Application.Commands.Application.Application;

public class CreateApplicationMessageCommandResponse
{
    public Guid Id { get; set; }

    public List<NotificationDefinition> Notifications { get; set; } = new();
}

public class NotificationDefinition
{
    /// <summary>
    /// 
    /// </summary>
    public string TemplateName { get; set; } = default!;

    /// <summary>
    /// How the recipient should be resolved.
    /// </summary>
    public NotificationRecipientKind RecipientKind { get; set; }

    /// <summary>
    /// When RecipientKind == DirectEmail, this is populated by the inner API.
    /// When RecipientKind is a system mailbox this will be null.
    /// </summary>
    public string? EmailAddress { get; set; }
}

public enum NotificationRecipientKind
{
    // Outer API uses config to resolve these to actual addresses:
    QfauMailbox,
    OfqualMailbox,
    SkillsEnglandMailbox,

    // Inner API must provide EmailAddress:
    DirectEmail
}

